using UnityEngine;

public class FlavorClick : MonoBehaviour
{
    public IceCreamSupply iceCreamSupply; // Assign your ice cream supply
    public GameObject restockButton; // Drag your restock button here in the Inspector

    void OnMouseDown()
    {
        // Prevent scooping if restock button is visible
        if (restockButton != null && restockButton.activeInHierarchy)
        {
            Debug.Log("Can't scoop - restock menu is open!");
            return;
        }

        if (iceCreamSupply == null)
        {
            Debug.LogError("IceCreamSupply reference not set!");
            return;
        }

        if (iceCreamSupply.UseScoop())
        {
            Debug.Log("Scooped! Remaining: " + iceCreamSupply.currentScoops);
        }
        else
        {
            Debug.Log("Out of scoops!");
        }
    }
}