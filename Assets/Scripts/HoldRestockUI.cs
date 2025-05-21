using UnityEngine;
using UnityEngine.UI;

public class HoldRestockUI : MonoBehaviour
{
    public Slider progressSlider;
    public IceCreamSupply iceCreamSupply;

    private float holdTimer = 0f;
    private bool isFilling = false;

    void Update()
    {
        if (iceCreamSupply != null && iceCreamSupply.IsEmpty && iceCreamSupply.CanRestock())
        {
            isFilling = true;
        }
        else
        {
            isFilling = false;
            holdTimer = 0f;
        }

        if (isFilling)
        {
            holdTimer += Time.deltaTime;
            progressSlider.value = holdTimer;

            if (holdTimer >= 3f)
            {
                Debug.Log($"Auto-restocking {iceCreamSupply.name}...");

                if (iceCreamSupply.CanRestock())
                {
                    iceCreamSupply.Restock();
                    Debug.Log("Restock successful!");
                }

                holdTimer = 0f;
                progressSlider.value = 0f;
                iceCreamSupply = null; // clear so button hides until next supply is empty
            }
        }
        else
        {
            progressSlider.value = 0f;
        }
    }
}
