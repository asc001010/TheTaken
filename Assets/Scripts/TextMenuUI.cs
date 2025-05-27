using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TextMenuUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string nextSceneName;

    public enum MenuType { Start, Controls, Quit }
    public MenuType menuType;

    public GameObject controlsPanel;
    public GameObject brushEffect;

    public AudioClip hoverSound;
    public AudioClip toggleSound; // 🔔 추가: Controls 클릭 효과음

    private AudioSource audioSource;
    public static GameObject sharedControlsPanel;

    private void Start()
    {
        if (menuType == MenuType.Controls && controlsPanel != null)
            sharedControlsPanel = controlsPanel;

        if (brushEffect != null)
            brushEffect.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.6f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (brushEffect != null)
            brushEffect.SetActive(true);

        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (brushEffect != null)
            brushEffect.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (menuType != MenuType.Controls && sharedControlsPanel != null && sharedControlsPanel.activeSelf)
            sharedControlsPanel.SetActive(false);

        switch (menuType)
        {
            case MenuType.Start:
                AudioListener.pause = true;
                LoadingSceneControllerData.nextSceneName = nextSceneName;
                SceneManager.LoadScene("LoadingScene");
                break;

            case MenuType.Controls:
                if (controlsPanel != null)
                {
                    controlsPanel.SetActive(!controlsPanel.activeSelf);

                    // 🔊 추가: Controls 클릭 시 사운드 재생
                    if (toggleSound != null)
                        audioSource.PlayOneShot(toggleSound);
                }
                break;

            case MenuType.Quit:
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                break;
        }
    }
}