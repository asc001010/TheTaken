using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    public Image fadeImage; // ������ UI �̹���

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeThenLoad());
        }
    }

    private IEnumerator FadeThenLoad()
    {
        // ���� ȭ�� Ȱ��ȭ
        fadeImage.gameObject.SetActive(true);

        // ����� ����
        AudioListener.pause = true;

        // ��� ��� (0.1��)
        yield return new WaitForSeconds(0.1f);

        // �� ��ȯ
        //SceneManager.LoadScene(nextSceneName);

        // ������ �ε��� ���� �� �̸� ����
        LoadingSceneControllerData.nextSceneName = nextSceneName;

        // �ε� �� ȣ��
        SceneManager.LoadScene("LoadingScene");
    }
}

