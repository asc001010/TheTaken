using UnityEngine;
using System.Collections;

public class LetterMove : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject letterObject;     // ���� ������Ʈ (3D)
    public GameObject letterUI;         // ���� ���� UI (ĵ����)
    public float moveDuration = 0.5f;   // �̵� �ð�
    public Vector3 cameraForwardOffset = new Vector3(0f, 0f, 1f); // ī�޶� �� ��ġ ������

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShowing = false;

    void Start()
    {
        // ���� ���� ��ġ�� ȸ�� ����
        originalPosition = letterObject.transform.position;
        originalRotation = letterObject.transform.rotation;

        letterUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShowing)
        {
            // ����ĳ��Ʈ ������ ���� Ŭ�� Ȯ�� (����)
            // ���� Ŭ���ߴٰ� �����ϰ� �ٷ� Show �Լ� ȣ��
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
