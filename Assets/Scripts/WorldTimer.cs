using UnityEngine;

public class WorldSpaceTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 10f;

    [Header("Visuals")]
    public SpriteRenderer ringSprite; // Assign your circular dial sprite here

    [Header("Optional Customer Logic")]
    public GameObject customerToNotify; // Optional: to trigger walkout

    private float timeLeft;
    private bool isRunning = false;

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;

        // Update dial scale
        float fraction = Mathf.Clamp01(timeLeft / totalTime);
        if (ringSprite != null)
            ringSprite.transform.localScale = Vector3.one * fraction;

        // Timer finished
        if (timeLeft <= 0f)
        {
            isRunning = false;
            if (ringSprite != null)
                ringSprite.transform.localScale = Vector3.zero;

            Debug.Log("⏰ Timer finished!");

            // Optional: auto walk out customer
            if (customerToNotify != null)
            {
                CustomerMovement cm = customerToNotify.GetComponent<CustomerMovement>();
                if (cm != null)
                {
                    cm.WalkOut();
                }
            }
        }
    }

    // 🔁 Start the timer fresh, linked to a customer (optional)
    public void StartTimerForCustomer(GameObject customer)
    {
        customerToNotify = customer;
        ResetTimer();
        StartTimer();
    }

    // Start without customer logic
    public void StartTimer()
    {
        timeLeft = totalTime;
        isRunning = true;

        if (ringSprite != null)
            ringSprite.transform.localScale = Vector3.one;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timeLeft = totalTime;
        if (ringSprite != null)
            ringSprite.transform.localScale = Vector3.one;
    }
}
