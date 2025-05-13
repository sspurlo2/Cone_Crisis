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
        bool isCorrect = currentOrder.CheckOrder(playerFlavors);

        if (isCorrect){
            Debug.Log("Order correct!");
            playerFlavors.Clear();
            //currentOrder.receiptCube.SetActive(false);

            if (correctOrderClips.Length > 0){
                AudioClip randomClip = correctOrderClips[Random.Range(0, correctOrderClips.Length)];
                audioSource.PlayOneShot(randomClip);
            }

        } else {
            Debug.Log("Incorrect order");

            if (incorrectOrderClips.Length > 0) {

                CustomerMovement customer = FindFirstObjectByType<CustomerMovement>();
                currentOrder.currentCustomer.Pay();
                AudioClip randomClip = incorrectOrderClips[Random.Range(0, incorrectOrderClips.Length)];
                audioSource.PlayOneShot(randomClip);
            }
        }

        return isCorrect;
    }
}