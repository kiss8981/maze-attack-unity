using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textMesh;
    private Player player;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() { 
        if (player.startGame)
        {
            textMesh.text = TimeSpan.FromSeconds(player.time).ToString(@"mm\:ss");
        }
    }
}
