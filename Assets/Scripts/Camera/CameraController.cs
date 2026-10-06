using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("Cinemachine Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform playerTarget;

    private Coroutine returnRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
    }

    private void Start()
    {
        if (playerTarget == null && virtualCamera != null)
        {
            playerTarget = virtualCamera.Follow;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }

        if (virtualCamera != null)
        {
            virtualCamera.Follow = newTarget;
            virtualCamera.LookAt = newTarget;
        }
    }

    public void ReturnToPlayer(float delay = 2f)
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
        }
        returnRoutine = StartCoroutine(ReturnRoutine(delay));
    }

    private IEnumerator ReturnRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (virtualCamera != null && playerTarget != null)
        {
            virtualCamera.Follow = playerTarget;
            virtualCamera.LookAt = playerTarget;
        }

        returnRoutine = null;
    }
}
