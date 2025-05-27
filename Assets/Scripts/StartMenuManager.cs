using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject controlsPanel;  // Controls 패널을 드래그해서 연결
    public string nextSceneName;
    public void OnStartButton()
    {
        // "GameScene"은 네가 이동할 씬 이름
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnControlsButton()
    {
        // Controls 패널 토글
        controlsPanel.SetActive(!controlsPanel.activeSelf);
    }

    public void OnQuitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료용
#endif
    }
}