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
        yield return null; // ù ������ ��� (UI ǥ�� ����)

        string nextScene = LoadingSceneControllerData.nextSceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);

        operation.allowSceneActivation = false;

        // ���� �ε� �Ϸ���� ��ٸ�
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // UX�� ���� �ణ ���
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
