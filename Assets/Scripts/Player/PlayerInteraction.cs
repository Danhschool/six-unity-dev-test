using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3.0f;
    [SerializeField] private float checkInterval = 0.1f;

    private float timer = 0f;
    private SoccerBall currentNearestBall;

    public event Action<SoccerBall> OnEnterBallRange;
    public event Action OnExitBallRange;

    public SoccerBall CurrentBall => currentNearestBall;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckNearbyBall();
        }
    }

    private void CheckNearbyBall()
    {
        if (BallManager.Instance == null) return;

        SoccerBall nearest = BallManager.Instance.GetNearestBall(transform.position, interactDistance);

        if (nearest != currentNearestBall)
        {
            currentNearestBall = nearest;

            if (currentNearestBall != null)
            {
                OnEnterBallRange?.Invoke(currentNearestBall);
            }
            else
            {
                OnExitBallRange?.Invoke();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = currentNearestBall != null ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);

        if (currentNearestBall != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentNearestBall.Position);
        }
    }
}
