using UnityEngine;
using System.Collections.Generic;

public class PlayerStack : MonoBehaviour
{
    public List<string> playerFlavors = new List<string>();
    public CustomerOrder currentOrder;
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

    public void SetCurrentOrder(CustomerOrder order)
    {
        currentOrder = order;
    }
    public bool TrySubmitOrder()
    {
        Debug.Log("TrySubmitOrder() called!");

        if (currentOrder == null)
        {
            Debug.LogWarning("currentOrder is null.");
            return false;
        }

        if (currentOrder.currentCustomer == null)
        {
            Debug.LogWarning("currentOrder.currentCustomer is null.");
            return false;
        }

        Debug.Log("currentOrder and customer are valid.");

        bool isCorrect = currentOrder.CheckOrder(playerFlavors);
        Debug.Log("Order checked: " + isCorrect);
        Debug.Log("Expected: " + string.Join(", ", currentOrder.flavorOrder));
        Debug.Log("Player:   " + string.Join(", ", playerFlavors));

        // Hide the receipt regardless
        if (currentOrder.receiptCube != null)
            currentOrder.receiptCube.SetActive(false);

        if (isCorrect)
        {
            Debug.Log("Order correct!");
            PlayRandomClip(correctOrderClips);

            currentOrder.currentCustomer.MoveToRegister(); // Correct = pay + leave

            FindObjectOfType<StarRatingDisplay>()?.IncreaseRating(1f);
        }
        else
        {
            Debug.Log("Incorrect order!");
            PlayRandomClip(incorrectOrderClips);

            currentOrder.currentCustomer.WalkOut(); // Wrong = walk out (no pay)

            FindObjectOfType<StarRatingDisplay>()?.IncreaseRating(-0.5f);
        }

        playerFlavors.Clear();
        MoveNextCustomerInLine();

        return isCorrect;
    }


    void PlayRandomClip(AudioClip[] clipArray)
    {
        if (clipArray.Length > 0 && audioSource != null)
        {
            AudioClip clip = clipArray[Random.Range(0, clipArray.Length)];
            Debug.Log("Playing clip: " + clip.name);
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Clip array empty or AudioSource missing.");
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
