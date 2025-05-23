using UnityEngine;

public class CustomerProximityTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the player is near and tutorial is at step 0
        if (other.CompareTag("Player") && TutorialManager.Instance.step == 0)
        {
            TutorialManager.Instance.AdvanceStep(); // Advance to step 1
        }
    }
}