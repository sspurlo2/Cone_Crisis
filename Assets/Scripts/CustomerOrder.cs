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
        Debug.Log($"[ORDER ZONE] Trigger entered by: {other.name}");

        if (other.CompareTag("Customer"))
        {
            Debug.Log("Customer entered order zone — Generating order");

            currentCustomer = other.GetComponent<CustomerMovement>();
            if (currentCustomer == null)
            {
                Debug.LogWarning("Could not find CustomerMovement on " + other.name);
                return;
            }

            receiptCube?.SetActive(true);
            GenerateOrder();
            DisplayOrder();

            Debug.Log("Generated order: " + string.Join(", ", flavorOrder));

            PlayerStack playerStack = FindFirstObjectByType<PlayerStack>();
            if (playerStack != null)
            {
                playerStack.currentOrder = this;
                Debug.Log(" PlayerStack.currentOrder set");
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

        Debug.Log("Generated order: " + string.Join(", ", flavorOrder));
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
        Debug.Log(" Checking Order...");
        Debug.Log("Expected: " + (flavorOrder.Count > 0 ? string.Join(", ", flavorOrder) : "EMPTY"));
        Debug.Log("Player:   " + (playerStack.Count > 0 ? string.Join(", ", playerStack) : "EMPTY"));

        if (playerStack.Count != flavorOrder.Count)
        {
            Debug.Log(" Count mismatch.");
            return false;
        }

        for (int i = 0; i < flavorOrder.Count; i++)
        {
            string expected = flavorOrder[i].Trim().ToLowerInvariant();
            string actual = playerStack[i].Trim().ToLowerInvariant();
            Debug.Log($"Comparing scoop {i + 1}: expected '{expected}', got '{actual}'");

            if (expected != actual)
            {
                Debug.Log("Mismatch found!");
                return false;
            }
        }

        Debug.Log("All scoops match!");
        return true;
    }



    void Awake()
{
    // Auto-assign currentCustomer if not set manually
    if (currentCustomer == null)
    {
        currentCustomer = GetComponentInParent<CustomerMovement>();
        if (currentCustomer == null)
            Debug.LogWarning("CustomerOrder could not auto-assign currentCustomer!");
        else
            Debug.Log(" CustomerOrder auto-assigned currentCustomer: " + currentCustomer.name);
    }
}
}

