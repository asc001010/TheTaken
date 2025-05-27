using UnityEngine;
using System.Collections;

public class LetterMove : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject letterObject;     // 편지 오브젝트 (3D)
    public GameObject letterUI;         // 편지 내용 UI (캔버스)
    public float moveDuration = 0.5f;   // 이동 시간
    public Vector3 cameraForwardOffset = new Vector3(0f, 0f, 1f); // 카메라 앞 위치 오프셋

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShowing = false;

    void Start()
    {
        // 편지 원래 위치와 회전 저장
        originalPosition = letterObject.transform.position;
        originalRotation = letterObject.transform.rotation;

        letterUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShowing)
        {
            // 레이캐스트 등으로 편지 클릭 확인 (생략)
            // 편지 클릭했다고 가정하고 바로 Show 함수 호출
            StartCoroutine(MoveLetterToCamera());
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isShowing)
        {
            StartCoroutine(ReturnLetterToOriginal());
        }
    }

    IEnumerator MoveLetterToCamera()
    {
        isShowing = true;

        Vector3 targetPos = playerCamera.transform.position + playerCamera.transform.TransformDirection(cameraForwardOffset);
        Quaternion targetRot = Quaternion.LookRotation(playerCamera.transform.forward);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            letterObject.transform.position = Vector3.Lerp(letterObject.transform.position, targetPos, elapsed / moveDuration);
            letterObject.transform.rotation = Quaternion.Slerp(letterObject.transform.rotation, targetRot, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        letterObject.transform.position = targetPos;
        letterObject.transform.rotation = targetRot;

        letterUI.SetActive(true);
    }

    IEnumerator ReturnLetterToOriginal()
    {
        letterUI.SetActive(false);

        float elapsed = 0f;
        Vector3 startPos = letterObject.transform.position;
        Quaternion startRot = letterObject.transform.rotation;

        while (elapsed < moveDuration)
        {
            letterObject.transform.position = Vector3.Lerp(startPos, originalPosition, elapsed / moveDuration);
            letterObject.transform.rotation = Quaternion.Slerp(startRot, originalRotation, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        letterObject.transform.position = originalPosition;
        letterObject.transform.rotation = originalRotation;

        isShowing = false;
    }
}
