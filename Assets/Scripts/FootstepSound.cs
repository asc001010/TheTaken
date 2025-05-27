using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioClip walkLoopSound;
    public AudioClip runLoopSound;

    public CharacterController characterController;
    public PlayerController playerController;

    private AudioSource audioSource;
    private AudioClip currentClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // ✅ 채팅 UI 열렸을 때 발소리 차단
        if (ChatUIManager.isChatOpen)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            return;
        }

        bool isGrounded = characterController.isGrounded;
        bool isMoving = playerController.IsMoving;
        bool isRunning = playerController.IsRunning;

        // 이동 중일 때만 처리
        if (isGrounded && isMoving)
        {
            AudioClip targetClip = isRunning ? runLoopSound : walkLoopSound;

            // 소리가 다르면 바꿔서 재생
            if (audioSource.clip != targetClip)
            {
                audioSource.Stop();
                audioSource.clip = targetClip;
                audioSource.Play();
                currentClip = targetClip;
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}