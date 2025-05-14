using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerStack : MonoBehaviour {
    public List<string> playerFlavors = new List<string>();
    public CustomerOrder currentOrder;
    public AudioClip[] correctOrderClips;
    public AudioClip[] incorrectOrderClips;
    private AudioSource audioSource;

    void Start () {
        audioSource = GetComponent<AudioSource>();

    }

    public void AddFlavor(string flavor) { // adds new scoop to cone
        playerFlavors.Add(flavor); 
    } 

    public bool TrySubmitOrder()
    {
        if (currentOrder == null || currentOrder.currentCustomer == null) {
            Debug.LogWarning("No current order or customer to check against.");
            return false;
        }

        bool isCorrect = currentOrder.CheckOrder(playerFlavors);

        if (isCorrect) {
            Debug.Log("Order correct!");
            playerFlavors.Clear();

            if (correctOrderClips.Length > 0){
                AudioClip randomClip = correctOrderClips[Random.Range(0, correctOrderClips.Length)];
                audioSource.PlayOneShot(randomClip);
            }

            currentOrder.currentCustomer.MoveToRegister();
            currentOrder.receiptCube.SetActive(false);
        }
        else {
            Debug.Log("Incorrect order!");

            if (incorrectOrderClips.Length > 0){
                AudioClip randomClip = incorrectOrderClips[Random.Range(0, incorrectOrderClips.Length)];
                audioSource.PlayOneShot(randomClip);
            }

            // have the customer leave (but no reward)
            currentOrder.currentCustomer.Pay();
            currentOrder.receiptCube.SetActive(false);
        }

        return isCorrect;
    }
}