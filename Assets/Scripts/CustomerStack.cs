using UnityEngine;
using System.Collections.Generic;

public class PlayerStack : MonoBehaviour {
    public List<string> playerFlavors = new List<string>();
    public CustomerOrder currentOrder;

    public void AddFlavor(string flavor) { // adds new scoop to cone
        playerFlavors.Add(flavor); 
    } 

    public void TrySubmitOrder() //check player stack against customer order
    {
        if (currentOrder.CheckOrder(playerFlavors)) {
            Debug.Log("Correct Order!"); //order matches
            playerFlavors.Clear();

            if (currentOrder.currentCustomer != null) {
                currentOrder.currentCustomer.StartLeaving();
            }

            currentOrder.receiptCube.SetActive(false);
        }
        else {
            Debug.Log("Incorrect order. Try again.");
        }
    }
}