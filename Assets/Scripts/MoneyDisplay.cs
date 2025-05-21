using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
        UpdateMoneyUI();
    }

    public void AddMoney(float amount)
    {
        GameManager.Instance.playerMoney += amount;
        UpdateMoneyUI();
    }

    public bool SubtractMoney(float amount)
    {
        if (GameManager.Instance.playerMoney >= amount)
        {
            GameManager.Instance.playerMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    public bool CanAfford(float amount)
    {
        return GameManager.Instance.playerMoney >= amount;
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = $"${GameManager.Instance.playerMoney:F2}";
    }

    public void UpdateDisplay()
    {
        if (GameManager.Instance == null || moneyText == null)
        {
            Debug.LogWarning("MoneyDisplay: Missing GameManager or moneyText!");
            return;
        }

        moneyText.text = $"${GameManager.Instance.playerMoney:F2}";
    }


}
