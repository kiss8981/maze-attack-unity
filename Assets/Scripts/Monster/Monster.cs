using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int maxHeart = 3;
    public float moveSpeed = 4;
    public GameObject target;

    [HideInInspector]
    public float time = 0.0f;

    [HideInInspector]
    public bool startGame = false;

    [HideInInspector]
    public int heart = 3;

    // Start is called before the first frame update
    void Start()
    {
        heart = maxHeart;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void TakeDamage(int damage)
    {
        if (heart > 0)
        {
            heart -= damage;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Movement()
    {
        Vector2 direction = target.transform.position - transform.position; // 플레이어와 AI 사이의 방향 벡터 계산
        float distance = direction.magnitude; // 플레이어와 AI 사이의 거리 계산

        if (distance > 1f) // 일정 거리 이상일 때만 이동
        {
            direction.Normalize(); // 방향 벡터 정규화
            Vector2 movement = direction * moveSpeed * Time.deltaTime; // 이동 벡터 계산

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, movement.magnitude); // AI와 이동 벡터 사이에 충돌이 있는지 체크
            if (hit.collider == null || hit.collider.CompareTag("Player")) // 충돌이 없거나 플레이어와 충돌한 경우에만 이동
            {
                transform.Translate(movement); // AI 이동
            }
        }
    }
}
