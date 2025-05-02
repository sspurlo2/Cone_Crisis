using UnityEngine;

public class IceCreamTub : MonoBehaviour
{
    public IceCreamSupply supplyScript;
    private float holdTime = 3f;
    private float holdTimer = 0f;
    private bool isHolding = false;

    void OnMouseDown()
    {
        if (supplyScript.CanRestock())
            isHolding = true;
    }

    void OnMouseUp()
    {
        isHolding = false;
        holdTimer = 0f;
    }

    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                supplyScript.Restock();
                isHolding = false;
                holdTimer = 0f;
            }
        }
    }
}
