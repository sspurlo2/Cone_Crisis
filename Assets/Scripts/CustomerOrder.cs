using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; 

public class CustomerOrder : MonoBehaviour {
    //public Text orderText; //assign in inspector
    public GameObject receiptCube; //receipt cube in unity
    public TextMeshPro receiptText; 
    public List<string> flavorOrder = new List<string>();
    public CustomerMovement currentCustomer; // store customer who ordered


    private void Start() {
        //receiptCube.SetActive(false); //hide receipt at start
        receiptText.text = "waiting for order...";
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Customer")) {
            Debug.Log("Customer entered trigger");
            receiptCube.SetActive(true);
            GenerateOrder();
            DisplayOrder();
            currentCustomer = other.GetComponent<CustomerMovement>();

            PlayerStack playerStack = FindFirstObjectByType<PlayerStack>();
            if (playerStack != null) {
                playerStack.currentOrder = this;
            }
        }
    }

    void GenerateOrder() {
        flavorOrder.Clear();
        string[] flavors = { "Strawberry", "Vanilla", "Chocolate", "Blueberry", "Mango"}; //flavor options
        int numScoops = Random.Range(1, 4); //rangeee 1-3 scoops bc min is inclusive max is exclusive
        for (int i = 0; i < numScoops; i++) {
            string randomFlavor = flavors[Random.Range(0, flavors.Length)]; //only one flavor for tutorial
            flavorOrder.Add(randomFlavor);
        }  
    }

    void DisplayOrder() {
        receiptText.text = "Order:\n";
        for (int i = 0; i < flavorOrder.Count; i++) {
            receiptText.text += $"Scoop {i + 1}: {flavorOrder[i]}\n";
        }
    }

    public bool CheckOrder(List<string> playerStack) { //checks if order matches players stack
        if (playerStack.Count != flavorOrder.Count) return false;
        for (int i = 0; i < flavorOrder.Count; i++) {
            if (playerStack[i] != flavorOrder[i]) return false;
        }
        return true;

    }
}