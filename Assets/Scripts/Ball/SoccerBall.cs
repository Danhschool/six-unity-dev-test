using System;
using System.Collections;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    [SerializeField] private float flightDuration = 1.0f;
    [SerializeField] private float arcHeight = 3.5f;
    [SerializeField] private float spinSpeed = 720f;

    private bool isFlying = false;
    private bool isInGoal = false;

    public bool IsFlying => isFlying;
    public bool IsInGoal => isInGoal;
    public Vector3 Position => transform.position;

    private void OnEnable()
    {
        if (BallManager.Instance != null)
        {
            BallManager.Instance.Register(this);
        }
    }

    private void Start()
    {
        if (BallManager.Instance != null)
        {
            BallManager.Instance.Register(this);
        }
    }

    private void OnDisable()
    {
        if (BallManager.Instance != null)
        {
            BallManager.Instance.Unregister(this);
        }
    }

    public void KickTo(Vector3 targetPosition, Action onGoalReached = null)
    {
        if (isFlying || isInGoal) return;
        StartCoroutine(FlightRoutine(targetPosition, onGoalReached));
    }

    private IEnumerator FlightRoutine(Vector3 targetPosition, Action onGoalReached)
    {
        isFlying = true;
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < flightDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flightDuration);

            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
            float heightOffset = 4f * arcHeight * t * (1f - t);
            currentPos.y += heightOffset;

            transform.position = currentPos;
            transform.Rotate(Vector3.right, spinSpeed * Time.deltaTime, Space.Self);

            yield return null;
        }

        transform.position = targetPosition;
        isFlying = false;
        isInGoal = true;

        onGoalReached?.Invoke();
    }
}
