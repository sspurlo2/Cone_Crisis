using UnityEngine;

public class IceCreamSupply : MonoBehaviour
{
    public int maxScoops = 5;
    public int currentScoops;
    public MoneyDisplay moneyDisplay; // Assign this in the Inspector
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

    // Check if the player can restock (has $50)
    public bool CanRestock() {
        return moneyDisplay != null && moneyDisplay.CanAfford(50);
    }

    // Deduct money and refill scoops
    public void Restock() {
        if (CanRestock()) {
            bool paymentSuccess = moneyDisplay.SubtractMoney(50);
            if (paymentSuccess) {
                currentScoops = maxScoops;
                Debug.Log("Restocked! Scoops: " + currentScoops);
            }
        } else {
            Debug.Log("Not enough money to restock!");
        }
    }
}