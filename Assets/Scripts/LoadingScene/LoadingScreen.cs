using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    public string sceneName;

    private void Start()
    {
        // �񵿱������� ���� ���� �ε�
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        // ���� �� �񵿱� �ε�
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // �� �ε尡 �Ϸ���� ���� ����
        while (!asyncOperation.isDone)
        {
            // �ε� ���൵�� �����̴��� �ݿ�
            progressBar.value = asyncOperation.progress;

            yield return null;
        }
    }
}