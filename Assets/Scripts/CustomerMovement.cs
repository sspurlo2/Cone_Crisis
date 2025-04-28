using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public Transform targetPoint; //target = counter
    public Transform exitPoint; //outside
    public float moveSpeed = 2f; 
    private bool hasOrdered = false; 

    private void Update() {
        if (targetPoint != null && !hasOrdered) {
            MoveTowardsTarget(targetPoint); 
        }
        else if (exitPoint != null && hasOrdered) {
            MoveTowardsTarget(exitPoint);
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
}