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
            progressSlider.value = holdTimer;

            if (holdTimer >= 3f)
            {
                // Restock ONLY if the player can afford it
                if (iceCreamSupply.CanRestock())
                {
                    iceCreamSupply.Restock();
                }
                ResetHold();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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
        ResetHold();
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        progressSlider.value = 0f;
    }
}