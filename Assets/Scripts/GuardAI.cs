using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GuardAI : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform[] patrolPoints;
    public float viewDistance = 15f;
    public float viewAngle = 120f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Audio Settings")]
    //public AudioClip footstepClip;
    public AudioSource audioSource;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private int patrolIndex = 0;

    private enum GuardState { Patrol, Chase }
    private GuardState currentState = GuardState.Patrol;

    public Camera guardCatchCamera;
    public GameObject gameOverUI;
    public GameObject mainCanvas;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.updateRotation = false;  // 회전 직접 처리
        agent.updateUpAxis = true;
        agent.angularSpeed = 720f;
        agent.acceleration = 20f;
        agent.stoppingDistance = 0.1f;

        GoToNextPoint();

        //audioSource.clip = footstepClip;
    }

    void Update()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
                Patrol();
                break;
            case GuardState.Chase:
                Chase();
                break;
        }

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);

        //// 발자국 소리 제어
        //if (footstepClip != null && audioSource != null)
        //{
        //    if (isMoving)
        //    {
        //        if (audioSource.clip == footstepClip && !audioSource.isPlaying)
        //            audioSource.Play();
        //    }
        //    else
        //    {
        //        if (audioSource.clip == footstepClip && audioSource.isPlaying)
        //            audioSource.Stop();
        //    }
        //}

        // ✅ 부드러운 회전 처리
        if (isMoving)
        {
            Quaternion lookRot = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    void Patrol()
    {
        if (CanSeePlayer())
        {
            currentState = GuardState.Chase;
            BackGroundChange.Instance.GuardSawPlayer();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void Chase()
    {
        if (CanSeePlayer())
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(player.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(path);
                animator.SetBool("isMoving", true);
            }
            else
            {
                StopChaseAndReturnToFirstPatrolPoint();
            }
        }
        else
        {
            StopChaseAndReturnToFirstPatrolPoint();
        }
    }

    void StopChaseAndReturnToFirstPatrolPoint()
    {
        BackGroundChange.Instance.GuardLostPlayer();

        agent.ResetPath();
        animator.SetBool("isMoving", false);

        patrolIndex = 0;
        agent.SetDestination(patrolPoints[patrolIndex].position);

        currentState = GuardState.Patrol;
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    bool CanSeePlayer()
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.6f;
        Vector3 targetPos = player.position + Vector3.up * 1.6f;
        Vector3 dirToPlayer = targetPos - eyePos;
        float distance = dirToPlayer.magnitude;

        if (distance < viewDistance)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < viewAngle / 2f)
            {
                if (!Physics.Raycast(eyePos, dirToPlayer.normalized, distance, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = GuardState.Patrol;

            agent.isStopped = true;
            agent.ResetPath();

            agent.velocity = Vector3.zero;

            if (mainCanvas != null)
                mainCanvas.SetActive(false);

            // 1. 플레이어 비활성화
            other.gameObject.SetActive(false);

            // 2. 기존 메인카메라 비활성화
            Camera mainCam = Camera.main;
            if (mainCam != null)
                mainCam.gameObject.SetActive(false);

            // 3. 가드 시점 카메라 활성화
            if (guardCatchCamera != null)
                guardCatchCamera.gameObject.SetActive(true);

            //BackGroundChange.Instance.CatchPlayer();

            animator.SetBool("isCatch", true);

            StartCoroutine(HandleCatchSequence());

            enabled = false;
            //SceneManager.LoadScene("StartScene");
        }
    }

    IEnumerator HandleCatchSequence()
    {
        yield return new WaitForSeconds(1f);

        // 1초 후에 음악/배경 등 변경
        BackGroundChange.Instance.CatchPlayer();

        yield return new WaitForSeconds(0.5f);

        // 1.5초 후 GameOver UI 표시
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}