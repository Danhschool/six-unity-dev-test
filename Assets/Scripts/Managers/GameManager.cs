using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerInteraction playerInteraction;

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

    private void Start()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (playerInteraction == null)
        {
            playerInteraction = FindObjectOfType<PlayerInteraction>();
        }

        if (uiManager != null)
        {
            uiManager.OnKickClicked += HandleKick;
            uiManager.OnAutoKickClicked += HandleAutoKick;
            uiManager.OnResetClicked += HandleReset;
        }

        if (playerInteraction != null)
        {
            playerInteraction.OnEnterBallRange += HandleBallInRange;
            playerInteraction.OnExitBallRange += HandleBallOutOfRange;
        }
    }

    private void HandleBallInRange(SoccerBall ball)
    {
        if (uiManager != null && ball != null && !ball.IsFlying && !ball.IsInGoal)
        {
            uiManager.SetKickButtonActive(true);
        }
    }

    private void HandleBallOutOfRange()
    {
        if (uiManager != null)
        {
            uiManager.SetKickButtonActive(false);
        }
    }

    private void HandleKick()
    {
        if (playerInteraction == null || playerInteraction.CurrentBall == null) return;

        SoccerBall ballToKick = playerInteraction.CurrentBall;
        if (ballToKick.IsFlying || ballToKick.IsInGoal) return;

        PerformKick(ballToKick);

        if (uiManager != null)
        {
            uiManager.SetKickButtonActive(false);
        }
    }

    private void HandleAutoKick()
    {
        if (playerInteraction == null || BallManager.Instance == null) return;

        SoccerBall furthestBall = BallManager.Instance.GetFurthestBall(playerInteraction.transform.position);
        if (furthestBall != null)
        {
            PerformKick(furthestBall);
        }
    }

    private void PerformKick(SoccerBall ball)
    {
        GoalPost nearestGoal = GoalPost.GetNearestGoal(ball.Position);
        if (nearestGoal == null) return;

        if (CameraController.Instance != null)
        {
            CameraController.Instance.SetTarget(ball.transform);
        }

        ball.KickTo(nearestGoal.TargetPosition, () =>
        {
            nearestGoal.PlayGoalEffect(2f);
            if (CameraController.Instance != null)
            {
                CameraController.Instance.ReturnToPlayer(2f);
            }
        });
    }

    private void HandleReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (uiManager != null)
        {
            uiManager.OnKickClicked -= HandleKick;
            uiManager.OnAutoKickClicked -= HandleAutoKick;
            uiManager.OnResetClicked -= HandleReset;
        }

        if (playerInteraction != null)
        {
            playerInteraction.OnEnterBallRange -= HandleBallInRange;
            playerInteraction.OnExitBallRange -= HandleBallOutOfRange;
        }
    }
}
