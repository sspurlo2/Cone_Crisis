using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WorldSpaceTimer : MonoBehaviour
{
    [Header("Countdown Settings")]
    public float totalTime = 10f;

    [Header("Visual Elements")]
    public SpriteRenderer ringSprite; // The visual ring
    public TextMeshProUGUI countdownText; // Countdown label

    [Header("Customer Logic")]
    public GameObject customerToNotify; // Assigned per customer
    public UnityEvent onTimerEnd; // Optional Unity event

    private float timeLeft;
    private bool isRunning = false;

    void Start()
    {
        ResetTimer(); // Optional: for testing
    }

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;

        // Update visual scale
        float fraction = Mathf.Clamp01(timeLeft / totalTime);
        if (ringSprite != null)
            ringSprite.transform.localScale = Vector3.one * fraction;

        // Update countdown text
        if (countdownText != null)
        {
            int secondsRemaining = Mathf.CeilToInt(timeLeft);
            countdownText.text = secondsRemaining.ToString();
        }

        // Check for timeout
        if (timeLeft <= 0f)
        {
            isRunning = false;
            if (ringSprite != null)
                ringSprite.transform.localScale = Vector3.zero;

            if (countdownText != null)
                countdownText.text = "0";

            Debug.Log("⏰ Timer finished!");
            onTimerEnd.Invoke();

            if (customerToNotify != null)
            {
                CustomerMovement cm = customerToNotify.GetComponent<CustomerMovement>();
                if (cm != null)
                {
                    cm.WalkOut();
                    FindObjectOfType<StarRatingDisplay>().IncreaseRating(-0.5f);
                }
            }

            // Move next customer forward
            PlayerStack playerStack = FindObjectOfType<PlayerStack>();
            if (playerStack != null)
            {
                playerStack.MoveNextCustomerInLine();
            }
        }
    }

    public void StartTimerForCustomer(GameObject customer)
    {
        customerToNotify = customer;
        ResetTimer();
        StartTimer();
    }

    public void StartTimer()
    {
        timeLeft = totalTime;
        isRunning = true;

        if (ringSprite != null)
            ringSprite.transform.localScale = Vector3.one;

        if (countdownText != null)
            countdownText.text = Mathf.CeilToInt(totalTime).ToString();
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

        if (countdownText != null)
            countdownText.text = Mathf.CeilToInt(totalTime).ToString();
    }
}
