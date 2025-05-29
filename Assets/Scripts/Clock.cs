using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;

    private float startTime;
    private float gameDayDuration;

    private DayManager dayManager;

    void Start()
    {
        // Find the DayManager and cache day duration
        dayManager = FindObjectOfType<DayManager>();
        if (dayManager != null)
        {
            gameDayDuration = dayManager.dayDuration;
        }
        else
        {
            Debug.LogError("Clock could not find DayManager!");
        }

        startTime = Time.time; // Mark start of the in-game day
    }

    void Update()
    {
        if (dayManager == null) return;

        float elapsed = Time.time - startTime;

        if (elapsed > gameDayDuration)
        {
            elapsed -= gameDayDuration;
            startTime = Time.time;
        }

        float dayFraction = elapsed / gameDayDuration;

        // Offset: 9AM = -270° rotation (starts pointing to the left)
        float hourRotation = (dayFraction * 360f) - 270f;
        float minuteRotation = ((dayFraction * 24f % 1f) * 360f) - 270f;
        float secondRotation = ((dayFraction * 24f * 60f % 1f) * 360f) - 270f;

        pointerHours.transform.localEulerAngles = new Vector3(0f, 0f, hourRotation);
        pointerMinutes.transform.localEulerAngles = new Vector3(0f, 0f, minuteRotation);
        pointerSeconds.transform.localEulerAngles = new Vector3(0f, 0f, secondRotation);
    }
}
