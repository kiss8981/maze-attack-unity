using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int maxHeart = 3;
    public float moveSpeed = 4;

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
    void Update() {

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

    public void Movement (Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }
}
