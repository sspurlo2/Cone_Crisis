using UnityEngine;
using TMPro;
using System.Collections;

public class DayManager : MonoBehaviour
{
    public TMP_Text dayText;               // The current day text
    public TMP_Text announcementText;      // The end of day announcement text
    public CanvasGroup announcementCanvas; // Reference to CanvasGroup for fade effect
    public float dayDuration = 360f;       // Duration of the day in seconds (6 minutes)
    public float transitionDelay = 4f;     // Time to show the end of day message
    public float fadeDuration = 1.5f;      // Time for fade-in effect

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

            // Record the starting money at the beginning of the day
            float moneyAtStart = GameManager.Instance.playerMoney;
            Debug.Log($"Day {currentDay} - Starting Money: ${moneyAtStart}");

            yield return new WaitForSeconds(dayDuration); // Wait for the day duration

            // Record the money at the end of the day
            float moneyAtEnd = GameManager.Instance.playerMoney;
            Debug.Log($"Day {currentDay} - Ending Money: ${moneyAtEnd}");

            // Calculate how much was earned during the day
            float earnedToday = moneyAtEnd - moneyAtStart;
            totalEarnings += earnedToday;

            // Display the end of day message with earned and total money
            announcementText.text = $"Day {currentDay} over!\nYou made ${earnedToday:F2} today.\nTotal: ${totalEarnings:F2}";

            // Fade in the announcement message
            yield return StartCoroutine(FadeInAnnouncement());

            // Wait before starting the next day
            yield return new WaitForSeconds(transitionDelay);

            // Advance to the next day
            currentDay++;
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

    void UpdateDayDisplay()
    {
        dayText.text = $"Day {currentDay}"; // Update the current day UI text
    }
}
