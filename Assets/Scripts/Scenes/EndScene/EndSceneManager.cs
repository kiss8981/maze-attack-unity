using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField, Header("플레이 타임을 표시할 텍스트")]
    private TextMeshProUGUI playTime;

    [SerializeField, Header("킬 카운트를 표시할 텍스트")]
    private TextMeshProUGUI killCount;

    // Start is called before the first frame update
    void Start()
    {
        string PlayTime = PlayerPrefs.GetString("PlayTime");
        string KillCount = PlayerPrefs.GetString("KillCount");

        playTime.text = "플레이 타임: " + PlayTime;
        killCount.text = "처치한 몬스터: " + KillCount + "마리";
    }

    // Update is called once per frame
    void Update() { }

    public void NextLevelStart()
    {
        int nodwLevel = PlayerPrefs.GetInt("Level");
        PlayerPrefs.SetInt("Level", nodwLevel + 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maze");
    }
}
