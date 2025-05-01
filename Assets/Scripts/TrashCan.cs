using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public Camera cam;
    public LayerMask trashCanLayer;
    public float maxDistance = 3f;
    public Scooper scooper; // Make sure this is assigned in Inspector

    void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        // Check if camera is assigned
        if (cam == null)
        {
            Debug.LogError("Camera not assigned in TrashCan!");
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, trashCanLayer))
        {
            ThrowAwayLeftHand();
            
            // Check if scooper is assigned before resetting
            if (scooper != null) 
            {
                scooper.ResetScooped();
                Debug.Log("Scooping reset successfully");
            }
            else
            {
                Debug.LogError("Scooper reference not set in TrashCan!");
            }
        }
    }
}

    void ThrowAwayLeftHand()
{
    GameObject playerCamObj = GameObject.Find("PlayerCam");
    if (playerCamObj == null)
    {
        Debug.LogError("PlayerCam not found in scene!");
        return;
    }

    foreach (Transform child in playerCamObj.transform)
    {
        if (child.CompareTag("Cone"))
        {
            Destroy(child.gameObject);
            break;
        }
    }
}
}