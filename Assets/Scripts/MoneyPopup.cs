using UnityEngine;
using TMPro;

public class MoneyPopup : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    public void SetAmount(int amount)
    {
        moneyText.text = "+$" + amount;
    }

    void Start()
    {
        Destroy(gameObject, 1.5f); // Matches animation duration
    }
}
