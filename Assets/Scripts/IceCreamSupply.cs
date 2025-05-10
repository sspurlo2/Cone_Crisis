using UnityEngine;

public class IceCreamSupply : MonoBehaviour
{
    public int maxScoops = 5;
    public int currentScoops;
    // Remove the moneyDisplay reference, since you're handling money through GameManager
    // public MoneyDisplay moneyDisplay; // Remove this line
    public bool IsEmpty => currentScoops <= 0; // True when out of scoops

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
    public void Restock() {
        if (CanRestock()) {
            bool paymentSuccess = SubtractMoney(50f);  // Deduct 50
            if (paymentSuccess) {
                currentScoops = maxScoops;
                Debug.Log("Restocked! Scoops: " + currentScoops);
            }
        } else {
            Debug.Log("Not enough money to restock!");
        }
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
