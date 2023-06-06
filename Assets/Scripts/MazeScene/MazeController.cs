using UnityEngine;
using System.Collections;

public class MazeController : MonoBehaviour
{
    [SerializeField]
    private GameObject GoalObject;
    private MazeGenerator mazeGenerator;

    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        mazeGenerator = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>();
        GenerateMaze();
    }

    void GenerateMaze()
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        int width = (level * 2) + 11 % 2 == 0 ? (level * 2) + 10 : (level * 2) + 11;
        int height = (level * 2) + 11 % 2 == 0 ? (level * 2) + 10 : (level * 2) + 11;
        mazeGenerator.MazeGenerate(width, height);
        GameStart();
    }

    void GameStart()
    {
        SetPlayerPosition();
        GenerateGoalObject();
        player.StartGame();
    }

    void SetPlayerPosition()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 mazeHalfSize =
            new Vector3(mazeGenerator.mazeSize.x, mazeGenerator.mazeSize.y, 0) / 2;
        player.transform.position =
            new Vector3(1, mazeGenerator.mazeSize.y - 2, 0)
            - mazeHalfSize
            + mazeGenerator.transform.position;
    }

    void GenerateGoalObject()
    {
        Vector3 mazeHalfSize =
            new Vector3(mazeGenerator.mazeSize.x, mazeGenerator.mazeSize.y, 0) / 2;
        GameObject Goal = Instantiate(
            GoalObject,
            new Vector3(mazeGenerator.mazeSize.x - 2, 1, 0)
                - mazeHalfSize
                + mazeGenerator.transform.position,
            Quaternion.identity
        );
        Goal.name = "Goal";
    }
}
