using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject restockButton; // Assign the RestockButton in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceCreamMachine"))
        {
            restockButton.SetActive(true); // Show button when near the machine
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IceCreamMachine"))
        {
            restockButton.SetActive(false); // Hide button when leaving
        }
    }
}