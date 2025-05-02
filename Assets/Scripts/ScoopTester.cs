using UnityEngine;

public class ScoopTester : MonoBehaviour
{
    public IceCreamSupply iceCreamSupply;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (iceCreamSupply.UseScoop())
            {
                Debug.Log("Scooped! Scoops left: " + iceCreamSupply.currentScoops);
            }
            else
            {
                Debug.Log("No scoops left! Restock needed.");
            }
        }
    }
}
