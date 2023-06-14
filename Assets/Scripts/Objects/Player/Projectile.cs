using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 몬스터에 닿으면 몬스터에게 데미지를 줌
        if (other.gameObject.CompareTag("Monster"))
        {
            other.gameObject.GetComponent<Monster>().TakeDamage(1);
            Destroy(this.gameObject);
        }

        // 벽에 닿으면 사라짐
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
