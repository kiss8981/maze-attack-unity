using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectLevelController : MonoBehaviour
{
    private int currentPage = 1;
    public Button firstLevelBtn;
    public Button secondLevelBtn;
    public Button thirdLevelBtn;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void OnClickNextPage()
    {
        currentPage += 1;
        ChangeLevelBtnText();
    }

    public void OnClickPrevPage()
    {
        if (currentPage == 1)
        {
            return;
        }
        currentPage -= 1;
        ChangeLevelBtnText();
    }

    private void ChangeLevelBtnText()
    {
        firstLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text =
            (currentPage - 1) * 3 + 1 + " 레벨";
        secondLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text =
            (currentPage - 1) * 3 + 2 + " 레벨";
        thirdLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text =
            (currentPage - 1) * 3 + 3 + " 레벨";
    }

    public void OnClickLevelButton(int btnNum)
    {
        PlayerPrefs.SetInt("Level", (currentPage - 1) * 3 + btnNum);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maze");
    }
}
