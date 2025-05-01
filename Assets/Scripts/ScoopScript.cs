using UnityEngine;

public class Scooper : MonoBehaviour
{
    public Camera cam; // Your main camera
    public LayerMask iceCreamLayer; // Layer only for ice cream tubs
    public GameObject vanillaConePrefab;
    public GameObject chocolateConePrefab;
    public GameObject strawberryConePrefab;
    public GameObject mangoConePrefab;
    public GameObject blueberryConePrefab;
    private bool scooped = false; // Flag to check if already scooped
    // Add more prefabs as needed

    public float maxDistance = 3f; // How close you need to be

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, iceCreamLayer))
            {
                Debug.Log("Clicked on " + hit.collider.name);
                if(scooped == false) // Check if already scooped
                {
                    SpawnCone(hit.collider.gameObject);
                }
            }
            if (Physics.Raycast(ray, out hit, maxDistance, CustomerMovement.customerLayer))
            {
                Debug.Log("Clicked on customer " + hit.collider.name);
                CustomerMovement customer = hit.collider.GetComponent<CustomerMovement>();
                if (customer != null)
                {
                    GiveCone(customer.gameObject);
                }
            }
            else
            {
                Debug.Log("Clicked on something else");

            }

        }
    }

    void SpawnCone(GameObject tub)
    {
        string flavor = tub.name.ToLower();

        GameObject coneToSpawn = null;

        if (flavor.Contains("vanilla"))
            coneToSpawn = vanillaConePrefab;
        else if (flavor.Contains("chocolate"))
            coneToSpawn = chocolateConePrefab;
        else if (flavor.Contains("strawberry"))
            coneToSpawn = strawberryConePrefab;
        else if (flavor.Contains("mango"))
            coneToSpawn = mangoConePrefab;
        else if (flavor.Contains("blueberry"))
            coneToSpawn = blueberryConePrefab;
        // Add more flavors...

        if (coneToSpawn != null)
        {
            Vector3 spawnPosition = transform.position 
                                  + transform.right * 0.2f
                                  + transform.up * 1.1f
                                  + transform.forward * 0.40f; 

            Instantiate(coneToSpawn, spawnPosition, transform.rotation, transform.parent);
            scooped = true; // Set scooped to true to prevent further scooping
        }
    }

    public void GiveCone(GameObject customerObj)
        {
            CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();
            
            
            if (scooped == true)
            {
                Debug.Log("Gave cone to customer!");
                customer.StartLeaving();
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
            

    public void ResetScooped()
    {
        scooped = false;
    }


}
