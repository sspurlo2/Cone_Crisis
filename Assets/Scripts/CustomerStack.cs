using UnityEngine;
using System.Collections.Generic;

public class PlayerStack : MonoBehaviour
{
    public List<string> playerFlavors = new List<string>(); // Flavors on the current cone
    public CustomerOrder currentOrder;                      // Reference to current customer order

    [Header("Order Feedback Sounds")]
    public AudioClip[] correctOrderClips;
    public AudioClip[] incorrectOrderClips;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Missing AudioSource on PlayerStack object!");
        }
    }

    public void AddFlavor(string flavor)
    {
        playerFlavors.Add(flavor);
    }

    public bool TrySubmitOrder()
    {
        if (currentOrder == null || currentOrder.currentCustomer == null)
        {
            Debug.LogWarning("No order or customer to check.");
            return false;
        }

        bool isCorrect = currentOrder.CheckOrder(playerFlavors);

        if (isCorrect)
        {
            Debug.Log("Order correct!");
            PlayRandomClip(correctOrderClips);

            playerFlavors.Clear();
            currentOrder.receiptCube.SetActive(false);
            currentOrder.currentCustomer.MoveToRegister();

            FindObjectOfType<StarRatingDisplay>()?.IncreaseRating(1f);
            MoveNextCustomerInLine();
        }
        else
        {
            Debug.Log("Incorrect order!");
            PlayRandomClip(incorrectOrderClips);

            playerFlavors.Clear();
            currentOrder.receiptCube.SetActive(false);
            currentOrder.currentCustomer.Pay();

            FindObjectOfType<StarRatingDisplay>()?.IncreaseRating(-0.5f);
            MoveNextCustomerInLine();
        }

        return isCorrect;
    }

    void PlayRandomClip(AudioClip[] clipArray)
    {
        if (clipArray.Length > 0 && audioSource != null)
        {
            AudioClip clip = clipArray[Random.Range(0, clipArray.Length)];
            audioSource.PlayOneShot(clip);
        }
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
}
