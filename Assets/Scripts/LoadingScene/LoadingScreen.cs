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
        // 비동기적으로 다음 씬을 로드
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        // 다음 씬 비동기 로드
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // 씬 로드가 완료되지 않은 동안
        while (!asyncOperation.isDone)
        {
            // 로드 진행도를 슬라이더에 반영
            progressBar.value = asyncOperation.progress;

            yield return null;
        }
    }
}