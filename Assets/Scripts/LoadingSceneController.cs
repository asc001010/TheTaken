using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    //public string nextSceneName;
    public TextMeshProUGUI loadingText;

    void Start()
    {
        StartCoroutine(BlinkText());
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null; // 첫 프레임 대기 (UI 표시 보장)

        string nextScene = LoadingSceneControllerData.nextSceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);

        operation.allowSceneActivation = false;

        // 실제 로딩 완료까지 기다림
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // UX를 위해 약간 대기
        yield return new WaitForSeconds(1f);

        operation.allowSceneActivation = true;

        //while (!operation.isDone)
        //{
        //    yield return null;
        //}
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            loadingText.enabled = true;
            yield return new WaitForSeconds(0.1f);
            loadingText.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
