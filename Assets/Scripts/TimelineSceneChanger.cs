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
            // ������ �ε��� ���� �� �̸� ����
            LoadingSceneControllerData.nextSceneName = nextSceneName;

            // �ε� �� ȣ��
            SceneManager.LoadScene("LoadingScene");
        }
    }
}

