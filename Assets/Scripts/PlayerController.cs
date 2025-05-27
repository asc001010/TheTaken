using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool IsMoving => currentMoveVelocity.magnitude > 0.1f;
    public bool IsRunning => Input.GetKey(KeyCode.LeftShift) && IsMoving;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float acceleration = 10f; // �̵� ���ӵ�
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;

    public CharacterController controller;
    public Transform cameraTransform;
    public Animator animator;

    public Vector3 velocity;
    private Vector3 currentMoveVelocity = Vector3.zero; // Lerp�� �̵� �ӵ�
    private float xRotation = 0f; // ���� ȸ�� ��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponent<Animator>();


        // ���콺 Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ȸ�� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �¿� ȸ�� (�÷��̾� ��ü ȸ��)
        transform.Rotate(Vector3.up * mouseX);

        // ���� ȸ�� (ī�޶� ȸ��)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 70f); // �þ� ����
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Ű���� �̵� ó�� (WASD)
        float inputX = Input.GetAxis("Horizontal"); // A, D
        float inputZ = Input.GetAxis("Vertical");   // W, S

        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;
        Vector3 moveDirection = transform.TransformDirection(inputDirection);


        // SHIFT Ű�� �ȱ�/�޸��� ��ȯ
        bool isMoving = inputDirection.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;
        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 targetVelocity = moveDirection * targetSpeed;

        // �ε巯�� ����/���� ó��
        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetVelocity, acceleration * Time.deltaTime);

        // ���� �̵� ����
        controller.Move(currentMoveVelocity * Time.deltaTime);

        // �߷� ����
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);



        // �ִϸ��̼� ����
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);

    }
}
