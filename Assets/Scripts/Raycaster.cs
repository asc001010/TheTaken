using UnityEngine;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 5f;
    public LayerMask interactLayer;
    public Image crosshairDot;
    public ImageSequenceManager menuUI; // ← 여기 연결 중요!

    public AudioClip openSound;
    private AudioSource audioSource;
    private GameObject lastHighlightedObject = null;
    private Color originalColor;

    void Start()
    {
      audioSource = GetComponent<AudioSource>();
      crosshairDot.color = new Color(1f, 1f, 1f, 0.2f);
    }
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Clue"))
            {
                if (crosshairDot != null)
                    crosshairDot.color = new Color(1f, 0f, 0f, 1f);;

                // 하이라이트 초기화
                if (lastHighlightedObject != null && lastHighlightedObject != hitObject)
                {
                    var prev = lastHighlightedObject.GetComponent<Renderer>();
                    if (prev != null)
                        prev.material.color = originalColor;
                    lastHighlightedObject = null;
                }

                // 현재 오브젝트 하이라이트
                if (lastHighlightedObject == null)
                {
                    var rend = hitObject.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        originalColor = rend.material.color;
                        rend.material.color = new Color(1f, 1f, 1f, 1f);;
                        lastHighlightedObject = hitObject;
                    }
                }

                // 마우스 클릭 시 삭제 + 메뉴 열기 허용
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.PlayOneShot(openSound);
                    Destroy(hitObject);
                    lastHighlightedObject = null;

                    if (menuUI != null)
                    {
                        menuUI.canOpenMenu = true;
                        Debug.Log("Clue 클릭됨 → I 키로 메뉴 열기 가능");
                    }
                }
            }
            else
            {
                ResetHighlight();
            }
        }
        else
        {
            ResetHighlight();
            if (crosshairDot != null)
                crosshairDot.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }

    void ResetHighlight()
    {
        if (lastHighlightedObject != null)
        {
            var rend = lastHighlightedObject.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = originalColor;
            lastHighlightedObject = null;
        }
    }
}