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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // freeze the rotation of the rigidbody to prevent it from tipping over
    }
   private void Update(){

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround); // check if the player is grounded
    
        MyInput();
        MovePlayer();
        SpeedControl();
        StopIfNoInput();
        if(grounded){
            rb.linearDamping = groundDrag; // apply drag when grounded
        }else{
            rb.linearDamping = 0; // remove drag when in the air
        }
    }

    private void MyInput(){
        horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        verticalInput = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow

    }

    private void MovePlayer(){
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; 
        moveDirection.Normalize(); // normalize the vector to prevent faster diagonal movement

        rb.AddForce(moveDirection * moveSpeed); // apply force to the rigidbody in the direction of movement
    }

private void SpeedControl(){
    Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

    if (flatVel.magnitude > moveSpeed){
        Vector3 limitedVel = flatVel.normalized * moveSpeed;
        rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
    }
}

private void StopIfNoInput(){
    if (horizontalInput == 0 && verticalInput == 0 && grounded){
        Vector3 stopVel = new Vector3(0, rb.linearVelocity.y, 0);
        rb.linearVelocity = stopVel;
    }
}
}
