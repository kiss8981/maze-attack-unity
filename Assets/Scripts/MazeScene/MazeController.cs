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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        GenerateMaze();
        SetPlayerPosition();
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Loading");
    }

    private void GenerateMaze()
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        int width = (level * 2) + 11 % 2 == 0 ? (level * 2) + 10 : (level * 2) + 11;
        int height = (level * 2) + 11 % 2 == 0 ? (level * 2) + 10 : (level * 2) + 11;
        mazeGenerator.MazeGenerate(width, height);
    }

    void SetPlayerPosition()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var startBlockNumber = mazeGenerator.GetStartBlockNumber();
        var startPosition = Block.GetPosition(startBlockNumber, mazeGenerator.mazeSize);
        player.transform.position = new Vector3(startPosition.x, 0.5f, startPosition.y);
    }
}
