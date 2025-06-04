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
        if (playerStack.Count != flavorOrder.Count)
            return false;

        for (int i = 0; i < flavorOrder.Count; i++)
        {
            string expected = flavorOrder[i].Trim().ToLowerInvariant();
            string actual = playerStack[i].Trim().ToLowerInvariant();

            if (expected != actual)
            {
                Debug.Log($"Mismatch at scoop {i + 1}: expected '{expected}', got '{actual}'");
                return false;
            }
        }

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

