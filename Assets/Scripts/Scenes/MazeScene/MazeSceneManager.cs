using UnityEngine;

public class MazeSceneManager : MonoBehaviour
{
    [SerializeField, Header("완주 지점 오브젝트")]
    private GameObject GoalObject;

    [SerializeField, Header("몬스터 오브젝트")]
    private GameObject MonsterObject;
    private Player player;
    private MazeGenerator mazeGenerator;
    private int monsterCount = 0;

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
        monsterCount = level * 2 + 1;
        mazeGenerator.MazeGenerate(width, height);
        GameStart();
    }

    void GameStart()
    {
        SetPlayerPosition();
        GenerateGoalObject();
        GenerateMonsterObject();
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

    void GenerateMonsterObject()
    {
        bool[,] randomArray = GetRandomArray(mazeGenerator.mazeSize.x, mazeGenerator.mazeSize.y);

        for (int x = 0; x < mazeGenerator.mazeSize.x; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeSize.y; y++)
            {
                if (!mazeGenerator.existWalls[x, y] || !randomArray[x, y] || monsterCount <= 0)
                    continue;

                Vector3 mazeHalfSize =
                    new Vector3(mazeGenerator.mazeSize.x, mazeGenerator.mazeSize.y, 0) / 2;
                Vector3 monsterPosition = new Vector3(x, y, 0) - mazeHalfSize + transform.position;

                monsterCount--;

                Instantiate(MonsterObject, monsterPosition, Quaternion.identity, transform);
            }
        }
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

    public bool[,] GetRandomArray(int width, int height)
    {
        bool[,] randomArray = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                randomArray[i, j] = (Random.value > 0.97f);
            }
        }

        return randomArray;
    }
}
