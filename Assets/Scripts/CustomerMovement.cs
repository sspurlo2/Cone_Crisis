using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public Transform targetPoint;
    public Transform exitPoint;
    public Transform registerPoint;

    public float moveSpeed = 2f;
    private bool hasOrdered = false;
    private bool hasPayed = false;
    public bool isAtRegister = false;

    public static LayerMask customerLayer;

    void Awake()
    {
        customerLayer = LayerMask.GetMask("customerLayer");

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

    void Update()
    {
        if (hasPayed && exitPoint != null)
        {
            MoveTowardsTarget(exitPoint);
            if (Vector3.Distance(transform.position, exitPoint.position) < 0.2f)
            {
                Destroy(gameObject); // Customer leaves
            }
        }
        else if (hasOrdered && registerPoint != null)
        {
            MoveTowardsTarget(registerPoint);
            isAtRegister = Vector3.Distance(transform.position, registerPoint.position) < 0.2f;
        }
        else if (targetPoint != null)
        {
            MoveTowardsTarget(targetPoint);
        }
    }

    void MoveTowardsTarget(Transform destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);

        Vector3 direction = (destination.position - transform.position).normalized;
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
        isAtRegister = false;
    }
}
