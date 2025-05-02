using UnityEngine;

public class IceCreamSupply : MonoBehaviour
{
    public int maxScoops = 5;
    public int currentScoops;

    void Start()
    {
        currentScoops = maxScoops;
    }

    public bool UseScoop()
    {
        if (currentScoops > 0)
        {
            currentScoops--;
            return true;
        }
        return false;
    }

    public bool CanRestock()
    {
        return GameManager.Instance.playerMoney >= 50f;
    }

    public void Restock()
    {
        if (CanRestock())
        {
            GameManager.Instance.playerMoney -= 50f;
            currentScoops = maxScoops;
        }
    }
}
