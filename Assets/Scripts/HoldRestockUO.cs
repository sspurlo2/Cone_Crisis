using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoldRestockUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider progressSlider;
    public IceCreamSupply iceCreamSupply; // Assigned dynamically via RestockButtonManager

    private bool isHolding = false;
    private float holdTimer = 0f;

    void Update()
{
    if (isHolding && iceCreamSupply != null)
    {
        holdTimer += Time.deltaTime;
        progressSlider.value = holdTimer; // Ensure slider max is set to 3 in Inspector

        if (holdTimer >= 3f)
        {
            // Debug log to verify restock conditions
            Debug.Log($"Attempting restock. CanRestock: {iceCreamSupply.CanRestock()}");
            
            if (iceCreamSupply.CanRestock())
            {
                iceCreamSupply.Restock();
                Debug.Log("Restock successful!");
            }
            else
            {
                Debug.LogWarning("Can't restock: Not enough money or supply not empty!");
            }

            ResetHold();
        }
    }
}

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down!"); 
        // Check if the supply exists AND can be restocked
        if (iceCreamSupply != null && iceCreamSupply.CanRestock())
        {
            isHolding = true;
            holdTimer = 0f;
            progressSlider.value = 0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up!");
        ResetHold();
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        if (progressSlider != null) progressSlider.value = 0f;
    }
}