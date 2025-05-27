using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    public Image fadeImage; // 검은색 UI 이미지

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeThenLoad());
        }
    }

    private IEnumerator FadeThenLoad()
    {
        // 검은 화면 활성화
        fadeImage.gameObject.SetActive(true);

        // 오디오 정지
        AudioListener.pause = true;

        // 잠깐 대기 (0.1초)
        yield return new WaitForSeconds(0.1f);

        // 씬 전환
        //SceneManager.LoadScene(nextSceneName);

        // 다음에 로드할 실제 씬 이름 저장
        LoadingSceneControllerData.nextSceneName = nextSceneName;

        // 로딩 씬 호출
        SceneManager.LoadScene("LoadingScene");
    }
}

