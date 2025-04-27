using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    public float groundDrag;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Jumping")]
    public float jumpForce = 5f; // Force applied when jumping
    public float airMultiplier = 0.5f; // Movement multiplier while in the air

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // freeze the rotation of the rigidbody to prevent it from tipping over
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround); // check if the player is grounded

        MyInput();
        MovePlayer();
        SpeedControl();
        StopIfNoInput();
        if (grounded)
        {
            rb.linearDamping = groundDrag; // apply drag when grounded
        }
        else
        {
            rb.linearDamping = 0; // remove drag when in the air
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded) // Check for jump input
        {
            Jump();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        verticalInput = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize(); // normalize the vector to prevent faster diagonal movement

        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Force); // apply force to the rigidbody in the direction of movement
        }
        else
        {
            rb.AddForce(moveDirection * moveSpeed * airMultiplier, ForceMode.Force); // reduced movement force in the air
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void StopIfNoInput()
    {
        if (horizontalInput == 0 && verticalInput == 0 && grounded)
        {
            Vector3 stopVel = new Vector3(0, rb.linearVelocity.y, 0);
            rb.linearVelocity = stopVel;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity before applying jump force
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply jump force
    }
}