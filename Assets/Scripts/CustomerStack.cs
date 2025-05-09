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
            Debug.Log("Order correct! Updating queue...");

            CustomerSpawner spawner = FindObjectOfType<CustomerSpawner>();
            if (spawner != null) {
                //log the queue before removal
                Debug.Log($"Queue before removal: {spawner.customerLine.Count} customers");
                for (int i = 0; i < spawner.customerLine.Count; i++) {
                    Debug.Log($"Customer {i}: {spawner.customerLine[i].gameObject.name}");
                }

                // Remove the first customer
                if (spawner.customerLine.Count > 0) {
                    spawner.customerLine.RemoveAt(0);
                    Debug.Log("First customer removed.");
                }

                //update remaining customers
                for (int i = 0; i < spawner.customerLine.Count; i++) {
                    if (i < spawner.queuePositions.Count) {
                        Debug.Log($"Moving customer {i} to {spawner.queuePositions[i].name}");
                        spawner.customerLine[i].MoveToFront(spawner.queuePositions[i]);
                    }
                }
            }

            //reset player stack & receipt
            playerFlavors.Clear();
            currentOrder.receiptCube.SetActive(false);
        } else {
            Debug.Log("Incorrect order. Try again.");
        }
    }
}
