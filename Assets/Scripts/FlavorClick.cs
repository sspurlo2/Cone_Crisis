using UnityEngine;

public class FlavorClick : MonoBehaviour
{
    public IceCreamSupply iceCreamSupply; // Drag this in for shared supply

    // In FlavorClick.cs
void OnMouseDown()
{
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
