using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHeart = 3;
    public float moveSpeed = 4;

    [HideInInspector]
    public float time = 0.0f;

    [HideInInspector]
    public bool startGame = false;

    [HideInInspector]
    public int heart = 3;

    void Start()
    {
        heart = maxHeart;
    }

    void Update()
    {
        MoveMent();
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
            Debug.Log("TakeDamage");
        }
        else
        {
            EndGame();
            // UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
        }
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void EndGame()
    {
        startGame = false;
    }

    public void AddTimer()
    {
        time += Time.deltaTime;
    }

    public float GetTime()
    {
        return time;
    }
}
