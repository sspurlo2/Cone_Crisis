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
                spawner.customerLine.RemoveAt(0); // remove the one that moved to register

                // move remaining customers forward in line
                for (int i = 0; i < spawner.customerLine.Count; i++) {
                    if (i < spawner.queuePositions.Count) {
                        spawner.customerLine[i].MoveToFront(spawner.queuePositions[i]);
                    }
                }
            }
        }

        currentOrder.receiptCube.SetActive(false);
        } else {
            Debug.Log("Incorrect order. Try again.");
        }
    }
}