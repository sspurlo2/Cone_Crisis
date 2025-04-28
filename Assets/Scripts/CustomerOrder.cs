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

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Customer")){
            receiptCube.SetActive(true); //show receipt when player steps on trigger
            GenerateOrder();
            DisplayOrder();

            currentCustomer = other.GetComponent<CustomerMovement>();

            /*CustomerMovement moveScript = other.GetComponent<CustomerMovement>();
            if (moveScript != null) {
                moveScript.StartLeaving();
            } */
        } 
    }

    void GenerateOrder() {
        flavorOrder.Clear();
        string[] flavors = { "Strawberry", "Vanilla", "Chocolate"}; //flavor options
        string randomFlavor = flavors[Random.Range(0, flavors.Length)]; //only one flavor for tutorial
        flavorOrder.Add(randomFlavor);
    }

    void DisplayOrder() {
        receiptText.text = "Order:\n";
        foreach (var flavor in flavorOrder) {
            receiptText.text += flavor + "\n";
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