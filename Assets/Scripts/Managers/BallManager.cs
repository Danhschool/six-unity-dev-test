using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }

    private readonly HashSet<SoccerBall> activeBalls = new HashSet<SoccerBall>();

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
    }

    public void Register(SoccerBall ball)
    {
        if (ball != null)
        {
            activeBalls.Add(ball);
        }
    }

    public void Unregister(SoccerBall ball)
    {
        if (ball != null)
        {
            activeBalls.Remove(ball);
        }
    }

    public SoccerBall GetNearestBall(Vector3 fromPos, float maxDistance)
    {
        SoccerBall nearest = null;
        float minSqrDist = maxDistance * maxDistance;

        foreach (var ball in activeBalls)
        {
            if (ball == null || ball.IsFlying || ball.IsInGoal) continue;

            float sqrDist = (fromPos - ball.Position).sqrMagnitude;
            if (sqrDist <= minSqrDist)
            {
                minSqrDist = sqrDist;
                nearest = ball;
            }
        }

        return nearest;
    }

    public SoccerBall GetFurthestBall(Vector3 fromPos)
    {
        SoccerBall furthest = null;
        float maxSqrDist = float.MinValue;

        foreach (var ball in activeBalls)
        {
            if (ball == null || ball.IsFlying || ball.IsInGoal) continue;

            float sqrDist = (fromPos - ball.Position).sqrMagnitude;
            if (sqrDist > maxSqrDist)
            {
                maxSqrDist = sqrDist;
                furthest = ball;
            }
        }

        return furthest;
    }
}
