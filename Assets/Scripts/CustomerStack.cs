using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerStack : MonoBehaviour {
    public List<string> playerFlavors = new List<string>();
    public CustomerOrder currentOrder;

    public void AddFlavor(string flavor) { // adds new scoop to cone
        playerFlavors.Add(flavor); 
    } 

    public bool TrySubmitOrder()
    {
        if (currentOrder.CheckOrder(playerFlavors))
        {
            Debug.Log("Order correct!");
            playerFlavors.Clear();
            currentOrder.receiptCube.SetActive(false);
            return true;
        }
        else
        {
            Debug.Log("Incorrect order. Try again.");
            return false;
        }
    }

}
