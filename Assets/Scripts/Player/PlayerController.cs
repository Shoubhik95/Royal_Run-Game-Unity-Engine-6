using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 12f;

    [Header("Movement Boundaries")]
    [SerializeField] private float xClamp = 2.5f;

    private Vector2 movement;
    private Rigidbody rigidBody;

    private float fixedZ;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody not found on Player!");
            return;
        }

        fixedZ = rigidBody.position.z;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (rigidBody == null)
            return;

        HandleMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        Vector3 currentPosition = rigidBody.position;

        float newX = currentPosition.x +
                     movement.x * moveSpeed * Time.fixedDeltaTime;

        newX = Mathf.Clamp(newX, -xClamp, xClamp);

        Vector3 newPosition = new Vector3(
            newX,
            currentPosition.y,
            fixedZ
        );

        rigidBody.MovePosition(newPosition);
    }
}