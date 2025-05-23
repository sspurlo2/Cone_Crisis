using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public TMP_Text tutorialText;
    public int step = 0;

    public bool isTutorial = true; // Set to false once tutorial ends

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
        Debug.Log($"[TutorialManager] Step advanced to: {step}");
        ShowStep();
    }

    public void ShowStep()
    {
        switch (step)
        {
            case 0:
                tutorialText.text = "Welcome to your first day! Let's run through a quick tutorial so you can succeed in your posistion!";
                break;
            case 1:
                tutorialText.text = "First, Lets start with clicking on the cone.";
                break;
            case 2:
                tutorialText.text = "Now, scoop the icecream. Be sure to click the right flavors, so that the customers do not get upset and leave. Note, if you click the wrong flavor, you can throw it away in the trash!";
                break;
            case 3:
                tutorialText.text = "Now, hand the icecream to the customer.";
                break;
            case 4:
                tutorialText.text = "Go to register and click on the customer so they can pay you.";
                break;
            case 5:
                ForceMessage("Congrats, you just served your first icecream cone. Good luck with your shift, you're already a star!", 6f);
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
        PlayerPrefs.SetInt("tutorialCompleted", 1);

        // Subscribe to scene loaded callback
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load main game scene
        SceneManager.LoadScene("Game_scene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Wait one frame before accessing UI
        StartCoroutine(RefreshRestockUIAfterLoad());
    }

    private System.Collections.IEnumerator RefreshRestockUIAfterLoad()
    {
        yield return null; // wait 1 frame

        RestockButtonManager manager = FindFirstObjectByType<RestockButtonManager>();
        if (manager != null)
        {
            manager.UpdateRestockUI(); // ensures proper positioning or hiding
        }
    }
}
