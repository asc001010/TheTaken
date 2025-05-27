using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class ImageSequenceManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;              // 표시할 패널 오브젝트
    public Image displayImage;            // 패널 내 이미지
    public Sprite clueImage;              // 보여줄 단 하나의 이미지

    [HideInInspector]
    public bool canOpenMenu = false;      // Raycaster에서 true로 설정
    public AudioClip openSound;
    private AudioSource audioSource;
    private bool isPanelOpen = false;

    
    private IEnumerator DelayedPanelToggle()
    {
        yield return new WaitForSeconds(0.3f);
        panel.SetActive(isPanelOpen);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //panel.SetActive(false);           // 처음엔 꺼짐
        if (displayImage != null && clueImage != null)
        {
            displayImage.sprite = clueImage;
        }   
        
    }


    void Update()
    {
        if (canOpenMenu && Input.GetKeyDown(KeyCode.I))
        {
           isPanelOpen = !isPanelOpen;
            audioSource.PlayOneShot(openSound);
            StartCoroutine(DelayedPanelToggle());
        }

        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isPanelOpen = false;
            panel.SetActive(false);
        }
    }

}