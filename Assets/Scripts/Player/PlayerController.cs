using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private string blendParamName = "Blend";

    [Header("Field Boundary")]
    [SerializeField] private BoxCollider boundaryBox;
    private float minX, maxX, minZ, maxZ;
    private bool hasBoundary = false;

    private CharacterController characterController;
    private Vector2 moveInput = Vector2.zero;
    private float verticalVelocity = 0f;
    private int blendHash;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        blendHash = Animator.StringToHash(blendParamName);
    }

    private void Start()
    {
        if (boundaryBox != null)
        {
            Bounds bounds = boundaryBox.bounds;
            minX = bounds.min.x;
            maxX = bounds.max.x;
            minZ = bounds.min.z;
            maxZ = bounds.max.z;
            hasBoundary = true;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAnimation();
        ClampPositionToBounds();
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            Vector3 motion = direction * moveSpeed;
            motion.y = verticalVelocity;
            characterController.Move(motion * Time.deltaTime);
        }
        else
        {
            Vector3 motion = new Vector3(0f, verticalVelocity, 0f);
            characterController.Move(motion * Time.deltaTime);
        }
    }

    private void HandleAnimation()
    {
        if (animator == null) return;

        float targetBlend = moveInput.sqrMagnitude > 0.001f ? 1f : 0f;

        animator.SetFloat(blendHash, targetBlend, 0.15f, Time.deltaTime);
    }
    private void ClampPositionToBounds()
    {
        if (!hasBoundary) return;
        Vector3 pos = transform.position;

        if (pos.x < minX || pos.x > maxX || pos.z < minZ || pos.z > maxZ)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            transform.position = pos;
        }
    }
}
