using System.Collections.Generic;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public static List<GoalPost> AllGoals { get; private set; } = new List<GoalPost>();

    [SerializeField] private Transform targetPoint;
    [SerializeField] private GameObject confettiPrefab;

    public Vector3 TargetPosition
    {
        get
        {
            if (targetPoint != null)
            {
                return targetPoint.position;
            }
            return transform.position + Vector3.up * 0.5f;
        }
    }

    private void OnEnable()
    {
        if (!AllGoals.Contains(this))
        {
            AllGoals.Add(this);
        }
    }

    private void OnDisable()
    {
        AllGoals.Remove(this);
    }

    public void PlayGoalEffect(float duration = 2f)
    {
        if (confettiPrefab != null)
        {
            Vector3 spawnPos = TargetPosition + Vector3.up * 1f;
            GameObject fx = Instantiate(confettiPrefab, spawnPos, Quaternion.identity);

            ParticleSystem[] particles = fx.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particles)
            {
                ps.Play();
            }

            Destroy(fx, duration);
        }
    }

    public static GoalPost GetNearestGoal(Vector3 position)
    {
        GoalPost nearest = null;
        float minSqrDist = float.MaxValue;

        foreach (var goal in AllGoals)
        {
            if (goal == null) continue;

            float sqrDist = (position - goal.TargetPosition).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                nearest = goal;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(TargetPosition, 0.6f);
    }
}
