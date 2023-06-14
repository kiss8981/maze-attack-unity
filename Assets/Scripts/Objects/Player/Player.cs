using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("플레이어 체력")]
    public int maxHeart = 3;

    [SerializeField, Header("플레이어 이동속도")]
    private float moveSpeed = 4;

    [SerializeField, Header("플레이어 공격 오브젝트")]
    private GameObject projectilePrefab;

    [SerializeField, Header("플레이어 공격 쿨타임")]
    private float attackCooldownTime = 1f;

    [SerializeField, Header("플레이어 피격 쿨타임")]
    private float damageCooldownTime = 10f;

    [HideInInspector]
    public float time = 0.0f;

    [HideInInspector]
    public bool startGame = false;

    [HideInInspector]
    public int heart = 3;

    [HideInInspector]
    public int killCount = 0;
    private float attackCooldownTimer = 0f;
    private float damageCooldownTimer = 0f;
    private Image damageEffectImage;

    void Start()
    {
        heart = maxHeart;
        damageEffectImage = GameObject.Find("DamgeEffect").GetComponent<Image>();
    }

    void Update()
    {
        Movement();
        CooldownTimer();

        if (Input.GetKeyDown(KeyCode.Mouse0) && attackCooldownTimer <= 0f)
        {
            LaunchProjectile();
        }

        if (startGame)
        {
            AddTimer();
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.right * -moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.up * -moveSpeed * Time.deltaTime;
        }
    }

    private void TakeDamage()
    {
        if (heart > 1)
        {
            damageCooldownTimer = damageCooldownTime;
            heart -= 1;
            ActivateDamageImage();
        }
        else
        {
            EndGame("GameOver");
        }
    }

    public void StartGame()
    {
        time = 0.0f;
        startGame = true;
    }

    public void EndGame(string type)
    {
        startGame = false;
        PlayerPrefs.SetString("PlayTime", TimeSpan.FromSeconds(time).ToString(@"mm\:ss"));
        PlayerPrefs.SetString("KillCount", killCount.ToString());
        UnityEngine.SceneManagement.SceneManager.LoadScene(type);
    }

    public void AddKillCount()
    {
        killCount += 1;
    }

    private void AddTimer()
    {
        time += Time.deltaTime;
    }

    private void CooldownTimer()
    {
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (damageCooldownTimer > 0f)
        {
            damageCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster") && damageCooldownTimer <= 0f)
        {
            TakeDamage();
        }
    }

    private void LaunchProjectile()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;

        Vector3 launchDirection =
            Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        launchDirection.Normalize();

        Vector3 projectilePosition = transform.position + launchDirection * 0.5f;

        GameObject projectile = Instantiate(
            projectilePrefab,
            projectilePosition,
            Quaternion.identity
        );
        projectile.transform.SetParent(this.gameObject.transform);
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, launchDirection);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(launchDirection * 10f, ForceMode2D.Impulse);

        attackCooldownTimer = attackCooldownTime;
    }

    private void ActivateDamageImage()
    {
        if (damageEffectImage != null)
        {
            damageEffectImage.color = new Color(1f, 0f, 0f, 1f);
            StartCoroutine(DelayedDisableDamageImage());
        }
    }

    private IEnumerator DelayedDisableDamageImage()
    {
        yield return new WaitForSeconds(0.5f);
        damageEffectImage.color = new Color(0f, 0f, 0f, 0f);
    }
}
