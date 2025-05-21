using UnityEngine;
using System.Collections;

public class CustomerMovement : MonoBehaviour
{
    public Transform targetPoint;
    public Transform exitPoint;
    public Transform registerPoint;

    public float moveSpeed = 2f;
    private bool hasOrdered = false;
    private bool hasPayed = false; // Has the customer paid?
    public static LayerMask customerLayer; // Layer for customers

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

        if (exitPoint == null)
        {
            exitPoint = GameObject.Find("CustomerExit")?.transform;
            if (exitPoint == null)
                Debug.LogError("Missing CustomerExit in scene!");
        }
    }

    private void Update()
    {
        if (hasPayed && exitPoint != null)
        {
            MoveTowardsTarget(exitPoint);
            if (Vector3.Distance(transform.position, exitPoint.position) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
        else if (hasOrdered && registerPoint != null)
        {
            MoveTowardsTarget(registerPoint);
        }
        else if (targetPoint != null)
        {
            MoveTowardsTarget(targetPoint);
        }
    }

    public void MoveTowardsTarget(Transform destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);

        Vector3 direction;

        if (destination == registerPoint)
        {
            // Always face the player when at register
            GameObject playerCam = GameObject.Find("PlayerCam");
            if (playerCam != null)
            {
                direction = (playerCam.transform.position - transform.position).normalized;
            }
            else
            {
                direction = (destination.position - transform.position).normalized;
            }
        }
        else
        {
            direction = (destination.position - transform.position).normalized;
        }

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    public void MoveToRegister()
    {
        hasOrdered = true;
    }

    public void Pay()
    {
        hasPayed = true;
    }

    public void MoveToFront(Transform target)
    {
        targetPoint = target;
        hasOrdered = false;
        hasPayed = false;
    }

    public void WalkOut()
    {
        if (exitPoint == null)
        {
            Debug.LogError($"Customer {gameObject.name} has no exitPoint!");
            return;
        }

        Debug.Log($"Customer {gameObject.name} is walking out.");
        hasOrdered = false;
        hasPayed = true;
        targetPoint = exitPoint;
    }
}
