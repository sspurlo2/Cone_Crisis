using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public Transform targetPoint; // Now accepts GameObjects
    public Transform exitPoint;
    public Transform registerPoint;

    public float moveSpeed = 2f; 
    private bool hasOrdered = false; 
    private bool hasPayed = false; //has the customer paid?
    public static LayerMask customerLayer; //layer for customers

    void Awake()
    {
        // Only runs once when object loads
        customerLayer = LayerMask.GetMask("customerLayer"); // Set your customer layer in Unity tags/layers

        if (registerPoint == null)
        {
            registerPoint = GameObject.Find("CustomerRegister")?.transform;
            if (registerPoint == null)
                Debug.LogError("Missing CustomerRegister in scene!");
        }
    }
    private void Update() {
        if (targetPoint != null && !hasOrdered) {
            MoveTowardsTarget(targetPoint); 
        }
        else if(hasOrdered) {
            MoveTowardsTarget(registerPoint); //move to exit point
        }
        else if (hasPayed) {
            MoveTowardsTarget(exitPoint); //move to exit point
            if (Vector3.Distance(transform.position, exitPoint.position) < 0.2f) {
                    Destroy(gameObject); //customer dissappears
                }
        }
    }

    private void MoveTowardsTarget(Transform destination) {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
        Vector3 direction = (destination.position - transform.position).normalized;
            if (direction != Vector3.zero) {
                transform.forward = direction; // face server
            }
        }

    public void StartLeaving() {
        hasOrdered = true;
    }
    public void Pay() {
        hasPayed = true;
    }
}