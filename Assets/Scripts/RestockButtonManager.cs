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

        // Position the button over the empty tub
        if (showButton)
        {
            // Slightly nudge the screen position upward
            tubScreenPosition.y += 50f; // pixels, not world units


            restockButton.GetComponent<RectTransform>().position = tubScreenPosition;
        }
    }
}