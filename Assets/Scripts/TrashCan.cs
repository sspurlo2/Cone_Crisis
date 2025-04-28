using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public Camera cam;
    public LayerMask trashCanLayer;
    public float maxDistance = 3f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, trashCanLayer))
            {
                Debug.Log("Clicked on Trash Can");

                ThrowAwayLeftHand();
            }
        }
    }

void ThrowAwayLeftHand()
{
    Transform playerCam = GameObject.Find("PlayerCam").transform; // Find your PlayerCam

    foreach (Transform child in playerCam)
    {
        if (child.CompareTag("Cone"))
        {
            Destroy(child.gameObject);
            break; // only destroy the first cone hand found
        }
    }
}
}