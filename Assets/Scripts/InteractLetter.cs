using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractLetter : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 5f;
    public LayerMask interactLayer;
    public Image crosshairDot;

    public AudioClip openSound;
    private AudioSource audioSource;
    private GameObject lastHighlightedObject = null;
    private Color originalColor;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShowing = false;
    public GameObject letterUI;

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

            if (isShowing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // 편지 UI가 켜져 있을 때 클릭하면 편지 닫기
                    CloseLetter(hitObject);
                }
            }

            if (hitObject.CompareTag("Clue"))
            {
                if (crosshairDot != null)
                    crosshairDot.color = new Color(1f, 0f, 0f, 1f); ;

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
                        rend.material.color = new Color(1f, 1f, 1f, 1f); ;
                        lastHighlightedObject = hitObject;
                    }
                }

                // 마우스 클릭 시 삭제 + 메뉴 열기 허용
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.PlayOneShot(openSound);
                    StartCoroutine(MoveLetterToCamera(hitObject));
                    lastHighlightedObject = null;

                    //if (menuUI != null)
                    //{
                    //    menuUI.canOpenMenu = true;
                    //    Debug.Log("Clue 클릭됨 → I 키로 메뉴 열기 가능");
                    //}
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

    IEnumerator MoveLetterToCamera(GameObject letter)
    {
        if (isShowing)
            yield break;

        isShowing = true;

        originalPosition = letter.transform.position;
        originalRotation = letter.transform.rotation;

        Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.forward * 0.2f; // 카메라 앞 m
        Quaternion targetRot = Quaternion.LookRotation(playerCamera.transform.forward);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            letter.transform.position = Vector3.Lerp(letter.transform.position, targetPos, elapsed / duration);
            letter.transform.rotation = Quaternion.Slerp(letter.transform.rotation, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        letter.transform.position = targetPos;
        letter.transform.rotation = targetRot;

        letter.GetComponent<Renderer>().enabled = false;
        // 편지 UI 켜기
        letterUI.SetActive(true);

        // 편지를 클릭해 닫는 로직도 필요하면 추가
    }

    public void CloseLetter(GameObject letter)
    {
        if (!isShowing)
            return;

        StartCoroutine(MoveLetterBack(letter));
    }

    IEnumerator MoveLetterBack(GameObject letter)
    {
        letterUI.SetActive(false);  // UI 끄기
        letter.GetComponent<Renderer>().enabled = true;

        float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startPos = letter.transform.position;
        Quaternion startRot = letter.transform.rotation;

        while (elapsed < duration)
        {
            letter.transform.position = Vector3.Lerp(startPos, originalPosition, elapsed / duration);
            letter.transform.rotation = Quaternion.Slerp(startRot, originalRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        letter.transform.position = originalPosition;
        letter.transform.rotation = originalRotation;

        isShowing = false;
    }
}

