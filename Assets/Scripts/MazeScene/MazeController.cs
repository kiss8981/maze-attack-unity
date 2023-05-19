using UnityEngine;

public class MazeController : MonoBehaviour
{
    // public MazeGenerator mazeGenerator;

    void Start()
    {
        Camera camera = Camera.main;
        int level = PlayerPrefs.GetInt("Level", 1);
        // mazeGenerator.GenerateMaze(level, level * 2);
        camera.transform.position = new Vector3(level * 2 * 1f / 2, level * 1f / 2, -1);
    }
}
