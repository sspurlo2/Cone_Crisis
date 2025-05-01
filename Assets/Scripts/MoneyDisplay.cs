using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class MoneyDisplay : MonoBehaviour
{
    // Reference to the TextMeshPro component to display the money
    public TMP_Text moneyText;  // For TextMeshPro

    // Example money value (could be updated dynamically in your game)
    private int moneyAmount = 100;

    void Start()
    {
        // Initialize the display when the script starts
        UpdateMoneyDisplay();
    }

    // Method to update the money display
    public void UpdateMoneyDisplay()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + moneyAmount.ToString();
        }
    }

    // Example method to add money
    public void AddMoney(int amount)
    {
        moneyAmount += amount;
        UpdateMoneyDisplay();
    }
}
