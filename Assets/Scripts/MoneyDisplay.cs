using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public TMP_Text moneyText;
    private int moneyAmount = 100;

    void Start() => UpdateMoneyDisplay();

    public void UpdateMoneyDisplay() {
        if (moneyText != null) moneyText.text = "Money: $" + moneyAmount;
    }

    // New method: Check if the player can afford a cost
    public bool CanAfford(int cost) {
        return moneyAmount >= cost;
    }

    // New method: Deduct money safely
    public bool SubtractMoney(int amount) {
        if (CanAfford(amount)) {
            moneyAmount -= amount;
            UpdateMoneyDisplay();
            return true; // Deduction successful
        }
        return false; // Not enough money
    }

    public void AddMoney(int amount) {
        moneyAmount += amount;
        UpdateMoneyDisplay();
    }
}