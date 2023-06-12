using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHeart = 3;
    public float moveSpeed = 4;
    public GameObject projectilePrefab;
    public float cooldownTime = 1f;

    [HideInInspector]
    public float time = 0.0f;

    [HideInInspector]
    public bool startGame = false;

    [HideInInspector]
    public int heart = 3;
    private float cooldownTimer = 0f;

    void Start()
    {
        heart = maxHeart;
    }

    void Update()
    {
        MoveMent();

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownTimer <= 0f)
        {
            LaunchProjectile();
        }

        if (startGame)
        {
            AddTimer();
        }
    }

    private void MoveMent()
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

    private void AddHealth()
    {
        if (heart < maxHeart)
        {
            heart += 1;
        }
    }

    private void TakeDamage()
    {
        if (heart > 0)
        {
            heart -= 1;
        }
        else
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void EndGame()
    {
        startGame = false;
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }

    public void AddTimer()
    {
        time += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            TakeDamage();
        }
    }

    private void LaunchProjectile()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Distance from the camera

        Vector3 launchDirection =
            Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        launchDirection.Normalize();

        Vector3 projectilePosition = transform.position + launchDirection * 0.5f;

        GameObject projectile = Instantiate(
            projectilePrefab,
            projectilePosition,
            Quaternion.identity
        );
        projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, launchDirection);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(launchDirection * 10f, ForceMode2D.Impulse);

        cooldownTimer = cooldownTime;
    }
}
