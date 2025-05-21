using UnityEngine;

public class IceCreamSupply : MonoBehaviour
{
    public int maxScoops = 5;
    public int currentScoops;
    // Remove the moneyDisplay reference, since you're handling money through GameManager
    // public MoneyDisplay moneyDisplay; // Remove this line
    public bool IsEmpty => currentScoops <= 0; // True when out of scoops
    public int restockCost = 50;


    void Start() => currentScoops = maxScoops;

    public bool UseScoop() {
        if (currentScoops > 0) {
            currentScoops--;
            return true;
        }
        Debug.Log("Out of scoops! Restock required.");
        return false;
    }

    // Check if the player can restock (has enough money in GameManager)
    public bool CanRestock() {
        return GameManager.Instance.playerMoney >= 50f; // Check if the player has enough money
    }

    // Deduct money and refill scoops
    public void Restock()
    {
        if (!CanRestock())
        {
            Debug.LogWarning("Tried to restock without enough money.");
            return;
        }

        GameManager.Instance.playerMoney -= restockCost;
        currentScoops = maxScoops;

        Debug.Log($"{gameObject.name} restocked. -${restockCost}. Player now has ${GameManager.Instance.playerMoney}");
        FindObjectOfType<MoneyDisplay>().UpdateDisplay(); // if you have such a method

    }


    // Subtract money from the player
    private bool SubtractMoney(float amount) {
        if (GameManager.Instance.playerMoney >= amount) {
            GameManager.Instance.playerMoney -= amount;
            return true; // Payment successful
        }
        return false; // Not enough money
    }
}
