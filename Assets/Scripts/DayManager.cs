using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    public TMP_Text dayText;               // The current day text
    public TMP_Text announcementText;      // The end of day announcement text
    public CanvasGroup announcementCanvas; // Reference to CanvasGroup for fade effect
    public float dayDuration = 360f;       // Duration of the day in seconds (6 minutes)
    public float transitionDelay = 4f;     // Time to show the end of day message
    public float fadeDuration = 1.5f;      // Time for fade-in effect
    public Image FadeImage;                // Fade out at End of Day

    private int currentDay = 1;
    private float totalEarnings = 0f;       // Total money earned across all days

    void Start()
    {
        StartCoroutine(DayCycle());
    }

    IEnumerator DayCycle()
    {
        while (true)
        {
            UpdateDayDisplay(); // Update current day on the UI
            announcementText.text = ""; // Clear the announcement text initially
            announcementCanvas.alpha = 0f; // Start with invisible announcement panel
            FadeImage.color = new Color(0, 0, 0, 0);

            // Record the starting money at the beginning of the day
            float moneyAtStart = GameManager.Instance.playerMoney;
            Debug.Log($"Day {currentDay} - Starting Money: ${moneyAtStart}");

            yield return new WaitForSeconds(dayDuration); // Wait for the day duration

            // Record the money at the end of the day
            float moneyAtEnd = GameManager.Instance.playerMoney;
            Debug.Log($"Day {currentDay} - Ending Money: ${moneyAtEnd}");

            float earnedToday = moneyAtEnd - moneyAtStart;

            // Get the star rating and tip percentage
            float starRating = FindObjectOfType<StarRatingDisplay>().GetRating(); 
            float tipPercent = Mathf.Lerp(0.10f, 0.50f, starRating / 5f);
            float tips = earnedToday * tipPercent;

            // Add tips to earnings for display and for total tracking
            float totalToday = earnedToday + tips;
            totalEarnings += totalToday;
            

            // Show breakdown in the announcement
            announcementText.text = $"Day {currentDay} over!\n" +
                $"Base: ${earnedToday:F2}\n" +
                $"Tips: ${tips:F2}\n" +
                $"Total: ${totalToday:F2}\n" +
                $"Overall: ${totalEarnings:F2}\n" +
                $"Star Rating: {starRating:F1} stars";
            FindObjectOfType<MoneyDisplay>().AddMoney(tips); // Update the money display
            // Fade in the announcement message
            yield return StartCoroutine(FadeInAnnouncement());
            MakeAllCustomersLeave();

            // Wait before starting the next day
            yield return new WaitForSeconds(transitionDelay);
            yield return StartCoroutine(FadeToBlack());

            // Advance to the next day
            currentDay++;
            UpdateDayDisplay();
            announcementText.text = "";
            announcementCanvas.alpha = 0f;

            yield return StartCoroutine(FadeFromBlack());
        }
    }

    IEnumerator FadeInAnnouncement()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            announcementCanvas.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration); // Smooth fade-in
            yield return null;
        }
        announcementCanvas.alpha = 1f; // Ensure the text is fully visible after fade
    }
    IEnumerator FadeToBlack()
    {
        float timer = 0f;
        Color color = FadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            FadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        FadeImage.color = color;
    }

    IEnumerator FadeFromBlack()
    {
        float timer = 0f;
        Color color = FadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            FadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        FadeImage.color = color;
    }


    void UpdateDayDisplay()
    {
        dayText.text = $"Day {currentDay}"; // Update the current day UI text
    }
    void MakeAllCustomersLeave()
{
    CustomerMovement[] customers = FindObjectsOfType<CustomerMovement>();
    foreach (CustomerMovement customer in customers)
    {
        customer.WalkOut();
    }
}

}
