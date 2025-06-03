using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    public TMP_Text dayText;
    public TMP_Text announcementText;
    public CanvasGroup announcementCanvas;
    public float dayDuration = 360f;
    public float transitionDelay = 4f;
    public float fadeDuration = 1.5f;
    public Image FadeImage;

    public Light directionalLight;
    public Gradient lightColorOverTime;
    public AnimationCurve lightIntensityOverTime;
    public GameObject[] nightLights;

    [Range(0f, 1f)]
    public float dayProgress = 0f;

    private float timer = 0f;
    private float dayLength;
    private int currentDay = 1;
    private float totalEarnings = 0f;

    void Start()
    {
        dayLength = dayDuration;
        StartCoroutine(DayCycle());
    }

    void Update()
    {
        timer += Time.deltaTime;
        dayProgress = timer / dayLength;
        UpdateLighting(dayProgress);
    }

    void UpdateLighting(float progress)
    {
        if (directionalLight != null)
        {
            directionalLight.color = lightColorOverTime.Evaluate(progress);
            directionalLight.intensity = lightIntensityOverTime.Evaluate(progress);
            directionalLight.transform.rotation = Quaternion.Euler(
                Mathf.Lerp(60f, -20f, progress),
                Mathf.Lerp(-30f, 30f, progress),
                0f
            );
        }

        RenderSettings.ambientIntensity = Mathf.Lerp(1f, 0.2f, progress);

        bool isNight = progress > 0.7f;
        foreach (GameObject light in nightLights)
        {
            if (light != null)
                light.SetActive(isNight);
        }
    }

    IEnumerator DayCycle()
    {
        while (true)
        {
            UpdateDayDisplay();
            announcementText.text = "";
            announcementCanvas.alpha = 0f;
            FadeImage.color = new Color(0, 0, 0, 0);

            float moneyAtStart = GameManager.Instance.playerMoney;
            Debug.Log($"Day {currentDay} - Starting Money: ${moneyAtStart}");

            yield return new WaitForSeconds(dayDuration);

            float moneyAtEnd = GameManager.Instance.playerMoney;
            float earnedToday = moneyAtEnd - moneyAtStart;
            float starRating = FindObjectOfType<StarRatingDisplay>().GetRating();
            float tipPercent = Mathf.Lerp(0.10f, 0.50f, starRating / 5f);
            float tips = earnedToday * tipPercent;
            float totalToday = earnedToday + tips;
            totalEarnings += totalToday;

            announcementText.text = $"Day {currentDay} over!\n" +
                                    $"Base: ${earnedToday:F2}\n" +
                                    $"Tips: ${tips:F2}\n" +
                                    $"Total: ${totalToday:F2}\n" +
                                    $"Overall: ${totalEarnings:F2}\n" +
                                    $"Star Rating: {starRating:F1} stars";
            FindObjectOfType<MoneyDisplay>().AddMoney(tips);

            yield return StartCoroutine(FadeInAnnouncement());
            MakeAllCustomersLeave();

            yield return new WaitForSeconds(transitionDelay);
            yield return StartCoroutine(FadeToBlack());

            // Reset day
            timer = 0f;
            dayProgress = 0f;
            UpdateLighting(0f);

            currentDay++;
            UpdateDayDisplay();
            announcementText.text = "";
            announcementCanvas.alpha = 0f;

            yield return StartCoroutine(FadeFromBlack());

            CustomerSpawner spawner = FindObjectOfType<CustomerSpawner>();
            if (spawner != null)
            {
                spawner.SetDay(currentDay);
                spawner.customerLine.Clear();
                spawner.ResetSpawner();
                PlayerStack stack = FindObjectOfType<PlayerStack>();
                if (stack != null)
                {
                    stack.Invoke("MoveNextCustomerInLine", 0.5f);
                }
            }
        }
    }

    IEnumerator FadeInAnnouncement()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            announcementCanvas.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        announcementCanvas.alpha = 1f;
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
        dayText.text = $"Day {currentDay}";
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
