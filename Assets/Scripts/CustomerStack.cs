using UnityEngine;
using System.Collections.Generic;

public class PlayerStack : MonoBehaviour
{
    public List<string> playerFlavors = new List<string>(); // Flavors on the current cone
    public CustomerOrder currentOrder;                      // Assigned when customer triggers order zone
    public AudioClip[] correctOrderClips;                   // Variants of success SFX
    public AudioClip[] incorrectOrderClips;                 // Variants of failure SFX

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void AddFlavor(string flavor)
    {
        playerFlavors.Add(flavor);
    }

    public bool TrySubmitOrder()
    {
        if (currentOrder == null || currentOrder.currentCustomer == null)
        {
            Debug.LogWarning("No current order or customer to check against.");
            return false;
        }

        bool isCorrect = currentOrder.CheckOrder(playerFlavors);

        if (isCorrect)
        {
            Debug.Log("Order correct!");
            PlayRandomClip(correctOrderClips);

            playerFlavors.Clear();
            currentOrder.receiptCube.SetActive(false);
            currentOrder.currentCustomer.MoveToRegister(); // Customer goes to pay
            FindObjectOfType<StarRatingDisplay>().IncreaseRating(1f);

            MoveNextCustomerInLine();
        }
        else
        {
            Debug.Log("Incorrect order!");
            PlayRandomClip(incorrectOrderClips);

            playerFlavors.Clear(); // Optional: You may or may not clear stack on failure
            currentOrder.receiptCube.SetActive(false);
            currentOrder.currentCustomer.Pay(); // Customer just walks out
            FindObjectOfType<StarRatingDisplay>().IncreaseRating(-.5f);

            MoveNextCustomerInLine();
        }

        return isCorrect;
    }

    public void MoveNextCustomerInLine()
    {
        CustomerSpawner spawner = FindObjectOfType<CustomerSpawner>();
        if (spawner == null) return;

        if (spawner.customerLine.Count > 0)
        {
            spawner.customerLine.RemoveAt(0);

            for (int i = 0; i < spawner.customerLine.Count; i++)
            {
                if (i < spawner.queuePositions.Count)
                {
                    spawner.customerLine[i].MoveToFront(spawner.queuePositions[i]);
                }
            }
        }
    }

    void PlayRandomClip(AudioClip[] clipArray)
    {
        if (clipArray.Length > 0 && audioSource != null)
        {
            AudioClip clip = clipArray[Random.Range(0, clipArray.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
