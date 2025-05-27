using UnityEngine;

public class BackGroundChange : MonoBehaviour
{
    public static BackGroundChange Instance;

    [Header("BGM Settings")]
    public AudioSource bgmSource;
    public AudioClip normalBGM;
    public AudioClip alertBGM;
    public AudioClip catchBGM;

    private AudioClip targetClip;
    private int alertingGuardCount = 0;
    private int catchGuard = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // 🎵 게임 시작 시 normalBGM 재생
        if (bgmSource != null && normalBGM != null)
        {
            bgmSource.clip = normalBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void GuardSawPlayer()
    {
        alertingGuardCount++;
        UpdateBGM();
    }

    public void GuardLostPlayer()
    {
        alertingGuardCount = Mathf.Max(0, alertingGuardCount - 1);
        UpdateBGM();
    }

    public void CatchPlayer()
    {
        catchGuard = 100;
        UpdateBGM();
    }

    private void UpdateBGM()
    {
        if (bgmSource == null) return;

        if (catchGuard == 0)
            targetClip = (alertingGuardCount > 0) ? alertBGM : normalBGM;
        else
            targetClip = catchBGM;

        if (bgmSource.clip == targetClip && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = targetClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}