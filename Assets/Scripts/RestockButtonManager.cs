using UnityEngine;
using UnityEngine.UI;

public class RestockButtonManager : MonoBehaviour
{
    public IceCreamSupply[] iceCreamSupplies;
    public MoneyDisplay moneyDisplay;
    public GameObject restockButton;
    public HoldRestockUI holdRestockScript;
    public Camera mainCamera; // Assign your main camera (e.g., Player's camera)

    void Update()
    {
        // Disable restock logic during tutorial
        if (GameManager.Instance != null && GameManager.Instance.isTutorial)
        {
            restockButton.SetActive(false); // Hide the button
            holdRestockScript.iceCreamSupply = null; // Clear assigned tub so it doesn't trigger automatic restock
            return; // Exit early
        }

        bool showButton = false;
        Vector3 tubScreenPosition = Vector3.zero;

        foreach (var supply in iceCreamSupplies)
        {
            if (supply.IsEmpty && moneyDisplay.CanAfford(50))
            {
                // Get the tub's world position and convert to screen space
                tubScreenPosition = mainCamera.WorldToScreenPoint(supply.transform.position);
                holdRestockScript.iceCreamSupply = supply;
                showButton = true;
                break; // Prioritize the first empty tub
            }
        }

        restockButton.SetActive(showButton);

        if (showButton)
        {
            tubScreenPosition.y += 50f; // Slight upward nudge in pixels
            restockButton.GetComponent<RectTransform>().position = tubScreenPosition;
        }
        else
        {
            holdRestockScript.iceCreamSupply = null; // Clear it if nothing is empty
        }
    }
}
