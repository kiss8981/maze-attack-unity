using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectSceneManager : MonoBehaviour
{
    private int currentPage = 1;

    [SerializeField, Header("첫번째 레벨 버튼")]
    public Button firstLevelBtn;

    [SerializeField, Header("두번째 레벨 버튼")]
    public Button secondLevelBtn;

    [SerializeField, Header("세번째 레벨 버튼")]
    public Button thirdLevelBtn;

    void Start() { }

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
