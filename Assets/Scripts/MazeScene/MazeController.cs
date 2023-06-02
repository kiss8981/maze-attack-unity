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
        Vector2 startBlock = mazeGenerator.getStartBlockNumber();
        Vector3 mazePosition = mazeGenerator.transform.position;
        var mazeHalfSize = new Vector3(mazeGenerator.mazeSize.x, mazeGenerator.mazeSize.y, 0) / 2;
        player.transform.position = new Vector3(1, 1, 0) - mazeHalfSize + mazeGenerator.GetMazeTransform().position;
    }
}
