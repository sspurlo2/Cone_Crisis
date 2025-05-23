using UnityEngine;

public class CustomerTutorialTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TutorialManager.Instance.step == 0)
        {
            TutorialManager.Instance.AdvanceStep(); // Move to step 1
        }
    }
}