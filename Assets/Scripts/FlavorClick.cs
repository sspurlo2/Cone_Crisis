using UnityEngine;

public class FlavorClick : MonoBehaviour
{
    public IceCreamSupply iceCreamSupply; // Drag this in for shared supply

    void OnMouseDown()
    {
        if (iceCreamSupply.UseScoop())
        {
            Debug.Log("Scooped from " + gameObject.name + "! Scoops left: " + iceCreamSupply.currentScoops);
            // TODO: Add visual or game logic for serving
        }
        else
        {
            Debug.Log("Out of scoops! Please restock.");
            // TODO: Show warning or disable interaction
        }
    }
}
