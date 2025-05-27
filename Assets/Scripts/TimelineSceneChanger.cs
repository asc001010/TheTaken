using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineSceneChanger : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName;

    void Start()
    {
        if (director != null)
        {
            director.stopped += OnTimelineFinished;
        }
    }

    void OnTimelineFinished(PlayableDirector pd)
    {
        if (pd == director)
        {
            // 다음에 로드할 실제 씬 이름 저장
            LoadingSceneControllerData.nextSceneName = nextSceneName;

            // 로딩 씬 호출
            SceneManager.LoadScene("LoadingScene");
        }
    }
}

