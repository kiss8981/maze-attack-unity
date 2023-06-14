using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private Player player;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        // 게임이 시작되면 타이머를 표시
        if (player.startGame)
        {
            textMesh.text = TimeSpan.FromSeconds(player.time).ToString(@"mm\:ss");
        }
    }
}
