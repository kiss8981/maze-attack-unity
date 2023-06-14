using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField, Header("하트 오브젝트")]
    private GameObject HeartObject;

    [SerializeField, Header("빈 하트 이미지")]
    private Sprite EmptyHeart;

    [SerializeField, Header("하트 이미지")]
    private Sprite FullHeart;
    private Player player;
    private GameObject HeartContainer;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        HeartContainer = GameObject.Find("HeartContainer");

        for (int i = 0; i < player.maxHeart; i++)
        {
            GameObject heart = Instantiate(
                HeartObject,
                new Vector2(
                    35f + (Screen.width / 1920f + i * 50f),
                    Screen.height - 40f
                ),
                Quaternion.identity
            );
            heart.transform.localScale = new Vector3(
                Screen.width / 1920f,
                Screen.height / 1080f,
                1f
            );
            heart.transform.SetParent(HeartContainer.transform);
            heart.name = "Heart" + i;
            heart.GetComponent<Image>().sprite = FullHeart;
        }
    }

    void Update()
    {
        if (player.heart < player.maxHeart)
        {
            GameObject heart = GameObject.Find("Heart" + (player.heart)).gameObject;
            heart.GetComponent<Image>().sprite = EmptyHeart;
        }
    }
}
