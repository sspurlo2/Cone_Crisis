using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CustomerOrder : MonoBehaviour
{
    public GameObject receiptCube;
    public TextMeshPro receiptText;
    public List<string> flavorOrder = new List<string>();
    public CustomerMovement currentCustomer;

    private void Start()
    {
        receiptText.text = "waiting for order...";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            Debug.Log("Customer entered order zone");

            currentCustomer = other.GetComponent<CustomerMovement>();
            if (currentCustomer == null) return;

            receiptCube.SetActive(true);
            GenerateOrder();
            DisplayOrder();

            // Register this order with the player stack (optional)
            PlayerStack playerStack = FindFirstObjectByType<PlayerStack>();
            if (playerStack != null)
            {
                playerStack.currentOrder = this;
            }

            // Start the countdown timer for this customer
            WorldSpaceTimer timer = FindObjectOfType<WorldSpaceTimer>();
            if (timer != null)
            {
                timer.StartTimerForCustomer(other.gameObject); // assign & start
            }
        }
    }

    void GenerateOrder()
    {
        flavorOrder.Clear();
        string[] flavors = { "Strawberry", "Vanilla", "Chocolate", "Blueberry", "Mango" };
        int numScoops = Random.Range(1, 4);

        for (int i = 0; i < numScoops; i++)
        {
            string randomFlavor = flavors[Random.Range(0, flavors.Length)];
            flavorOrder.Add(randomFlavor);
        }
    }

    void DisplayOrder()
    {
        receiptText.text = "Order:\n";
        for (int i = 0; i < flavorOrder.Count; i++)
        {
            receiptText.text += $"Scoop {i + 1}: {flavorOrder[i]}\n";
        }
    }

    public bool CheckOrder(List<string> playerStack)
    {
        if (playerStack.Count != flavorOrder.Count) return false;

        for (int i = 0; i < flavorOrder.Count; i++)
        {
            if (playerStack[i] != flavorOrder[i]) return false;
        }

        return true;
    }
}
