using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public TMP_Text tutorialText;
    public int step = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ShowStep();
    }

    public void AdvanceStep()
    {
        step++;
        ShowStep();
    }

    public void ShowStep()
    {
        switch (step)
        {
            case 0:
                tutorialText.text = "Walk to customer";
                break;
            case 1:
                tutorialText.text = "Click on the cone first!";
                break;
            case 2:
                tutorialText.text = "Now scoop the ice cream!";
                break;
            case 3:
                tutorialText.text = "Hand cone to customer!";
                break;
            case 4:
                tutorialText.text = "Go to register so they pay!";
                break;
            case 5:
                ForceMessage("Congrats! Good luck with the game!", 4f);
                Invoke(nameof(FinishTutorial), 4f); // Wait 4 seconds then start game
                break;
            default:
                tutorialText.text = "";
                break;
        }
    }

    public void ForceMessage(string message, float duration = 0f)
    {
        tutorialText.text = message;
        if (duration > 0f)
            StartCoroutine(HideAfterDelay(duration));
    }

    private System.Collections.IEnumerator HideAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        tutorialText.text = "";
    }

    public void FinishTutorial()
    {
        PlayerPrefs.SetInt("tutorialCompleted", 1); // So we can skip it next time
        SceneManager.LoadScene("Game_scene"); // Replace with your main game scene name
    }
}
