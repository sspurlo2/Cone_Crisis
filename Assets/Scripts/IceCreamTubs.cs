using UnityEngine;

public class IceCreamTub : MonoBehaviour
{
    public string flavorName; 

    private void OnMouseDown()
    {
        // When player clicks the tub
        PlayerStack player = FindObjectOfType<PlayerStack>();
        if (player != null)
        {
            player.AddFlavor(flavorName); // Add the flavor to the player's stack
            Debug.Log("Scooped: " + flavorName);
        }
    }
}
