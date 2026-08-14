using UnityEngine;

public class PlayerCollisionhandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Collision Settings")]
    [SerializeField] private float collisionCooldown = 1f;
    [SerializeField] private float adjustChangeMoveSpeedAmount = -2f;

    private const string HIT_TRIGGER = "Hit";

    private float cooldownTimer;

    private LevelGenerator levelGenerator;

    private void Awake()
    {
        // Unity 6 Recommended
        levelGenerator = FindFirstObjectByType<LevelGenerator>();

        if (levelGenerator == null)
        {
            Debug.LogError("LevelGenerator not found in the scene!");
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Collision immediately work kare
        cooldownTimer = collisionCooldown;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Sirf obstacle se collision
        if (!collision.gameObject.CompareTag("Obstacle"))
            return;

        // Cooldown
        if (cooldownTimer < collisionCooldown)
            return;

        // Speed kam karo
        if (levelGenerator != null)
        {
            levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
        }

        // Hit animation
        if (animator != null)
        {
            animator.SetTrigger(HIT_TRIGGER);
        }

        // Reset cooldown
        cooldownTimer = 0f;
    }
}