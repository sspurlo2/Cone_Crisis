using UnityEngine;
using UnityEngine.UI;

public class RestockButtonManager : MonoBehaviour
{
    public IceCreamSupply[] iceCreamSupplies;
    public MoneyDisplay moneyDisplay;
    public GameObject restockButton;
    public HoldRestockUI holdRestockScript;
    public Camera mainCamera;

    private bool initialized = false;

    void Start()
    {
        if (restockButton != null)
        {
            restockButton.SetActive(false); // Hide it early
        }

        // Start the update a little delayed to make sure scene is ready
        Invoke(nameof(InitAfterSceneLoad), 0.1f);
    }

    void Update()
    {
        if (initialized)
        {
            UpdateRestockUI();
        }
    }

    private void InitAfterSceneLoad()
    {
        initialized = true;
        UpdateRestockUI(); // Force initial check
    }

    public void UpdateRestockUI()
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.step < 5)
        {
            if (restockButton != null)
                restockButton.SetActive(false);
            return;
        }

        bool showButton = false;
        Vector3 tubScreenPosition = Vector3.zero;

        foreach (var supply in iceCreamSupplies)
        {
            if (supply.IsEmpty && moneyDisplay.CanAfford(50))
            {
                tubScreenPosition = mainCamera.WorldToScreenPoint(supply.transform.position);
                holdRestockScript.iceCreamSupply = supply;
                showButton = true;
                break;
            }
        }

        if (restockButton != null)
        {
            restockButton.SetActive(showButton);

            if (showButton)
            {
                tubScreenPosition.y += 50f;
                restockButton.GetComponent<RectTransform>().position = tubScreenPosition;
            }
        }
    }
}
