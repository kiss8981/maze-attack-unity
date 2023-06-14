using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField, Header("체력 (1일 경우 1방에 죽음)")]
    private int maxHeart = 3;

    [SerializeField, Header("이동 속도")]
    private float moveSpeed = 3.2f;

    [SerializeField, Header("벽을 피하기 위한 거리")]
    private float avoidDistance = 2f;

    [SerializeField, Header("추적할 태그")]
    private string targetTag = "Player";

    [SerializeField, Header("추적 범위")]
    public float detectionRadius = 5f;

    [HideInInspector]
    public float time = 0.0f;

    [HideInInspector]
    public bool startGame = false;

    [HideInInspector]
    public int heart = 3;
    private Transform target;
    private Vector3 previousPosition;
    private Rigidbody2D rb;
    private bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        heart = maxHeart;
    }

    private void FixedUpdate()
    {
        isColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        previousPosition = transform.position;
    }

    private void Movement()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance <= detectionRadius)
            {
                Vector2 direction = target.position - transform.position;
                rb.velocity = direction.normalized * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void TakeDamage(int damage)
    {
        if (heart > 0)
        {
            heart -= damage;
        }
        else
        {
            GameObject.Find("Player").GetComponent<Player>().AddKillCount();
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isColliding = true;
            Vector2 normal = collision.GetContact(0).normal;
            rb.velocity = Vector2.Reflect(rb.velocity, normal).normalized * moveSpeed;
        }
    }
}
