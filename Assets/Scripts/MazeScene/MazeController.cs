using UnityEngine;
using System.Collections;

public class MazeController : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator mazeGenerator;

    void Start()
    {
        MazeGenerate();
    }

    private void MazeGenerate()
    {
        GenerateMaze();
        SetPlayerPosition();
    }

    private void GenerateMaze()
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        int width = (level * 2) + 11 % 2 == 0 ? (level * 2) + 10 : (level * 2) + 11;
        int height = (level * 2) + 11 % 2 == 0 ? (level * 2) + 10 : (level * 2) + 11;
        mazeGenerator.MazeGenerate(width, height);
        SetPlayerPosition();
    }

    void SetPlayerPosition()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Block startBlock = mazeGenerator.GetStartBlockNumber();
        Vector3 mazePosition = mazeGenerator.transform.position;

        player.transform.position = startBlock.GetPosition(mazePosition, mazeGenerator.mazeSize);
    }
}
