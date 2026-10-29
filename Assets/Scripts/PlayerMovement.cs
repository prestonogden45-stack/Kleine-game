
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        if (rb == null || capsule == null)
        {
            Debug.LogError("Player needs a Rigidbody and Capsule Collider!");
            enabled = false;
        }
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        if (moveInput.sqrMagnitude > 1)
            moveInput.Normalize();

        grounded = Physics.Raycast(
            capsule.bounds.center,
            Vector3.down,
            capsule.bounds.extents.y + 0.15f
        );

        if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
            jumpPressed = true;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(
            moveInput.x, 0f, moveInput.y
        );

        Vector3 velocity = rb.linearVelocity;
        velocity.x = movement.x * speed;
        velocity.z = movement.z * speed;
        rb.linearVelocity = velocity;

        if (jumpPressed && grounded)
        {
            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );
        }

        jumpPressed = false;
    }
}
