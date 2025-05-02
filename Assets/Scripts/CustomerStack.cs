using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerStack : MonoBehaviour {
    public List<string> playerFlavors = new List<string>();
    public CustomerOrder currentOrder;

    public void AddFlavor(string flavor) { // adds new scoop to cone
        playerFlavors.Add(flavor); 
    } 

    public void TrySubmitOrder() {
        if (currentOrder.CheckOrder(playerFlavors)) {
            Debug.Log("Correct Order!");
            playerFlavors.Clear();

            if (currentOrder.currentCustomer != null) {
                currentOrder.currentCustomer.MoveToRegister();

                CustomerSpawner spawner = FindObjectOfType<CustomerSpawner>();
                if (spawner != null && spawner.customerLine.Count > 0) {
                    spawner.customerLine.RemoveAt(0);

                    if (spawner.customerLine.Count > 0) {
                        CustomerMovement nextCustomer = spawner.customerLine[0];
                        Vector3 frontPos = currentOrder.receiptCube.transform.position;
                        nextCustomer.MoveToFront(frontPos);
                    }
                }
            }

            currentOrder.receiptCube.SetActive(false);
        } else {
            Debug.Log("Incorrect order. Try again.");
        }
    }
}