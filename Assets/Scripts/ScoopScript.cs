using UnityEngine;

public class Scooper : MonoBehaviour
{
    public Camera cam; // Your main camera
    public LayerMask iceCreamLayer; // Layer only for ice cream tubs
    public LayerMask coneLayer; // Layer only for cones
    public GameObject cone; // Prefab for the cone to spawn
    private bool scooped = false; // Flag to check if already scooped
    private bool conePickedUp = false; // Flag to check if cone is picked up
    public Material blueberryMat;
    public Material chocolateMat;
    public Material mangoMat;
    public Material strawberryMat;
    public Material vanillaMat;
    private int scoopcount = 1; 


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
            if (conePickedUp == true)
            {
                // Get the IceCreamSupply component from the clicked object or its parent
                IceCreamSupply supply = hit.collider.GetComponentInParent<IceCreamSupply>();
                
                if (supply != null)
                {
                    // Check if a scoop is available
                    if (supply.UseScoop())
                    {
                        SpawnCone(hit.collider.gameObject); // Allow scooping
                        scooped = true; // Set scooped to tru
                    }
                    else
                    {
                        Debug.Log("No scoops left! Restock required.");
                        // Optional: Play a sound or show a UI warning
                    }
                }
                else
                {
                    Debug.LogWarning("No IceCreamSupply found on the clicked object.");
                }
            }
            else
            {
                Debug.Log("Cannot scoop: Already scooped or no cone picked up.");
            }
        }
            if (Physics.Raycast(ray, out hit, maxDistance, coneLayer))
            {
                Debug.Log("Clicked on " + hit.collider.name);
                if(scooped == false) // Check if already scooped
                {
                    PickUpCone(hit.collider.gameObject);
                }
            }
            if (Physics.Raycast(ray, out hit, maxDistance, CustomerMovement.customerLayer))
            {
                Debug.Log("Clicked on customer " + hit.collider.name);
                CustomerMovement customer = hit.collider.GetComponent<CustomerMovement>();
                if(!scooped) // Check if already scooped
                {
                    RingUp(customer.gameObject);
                }
                if (customer != null)
                {
                    GiveCone(customer.gameObject);
                    ResetScooped(); 

                }
                
            }

        }
    }

void SpawnCone(GameObject tub)
{
    string flavor = tub.name.ToLower();

    // Find "Hand With Scooper" -> then find "CreamConeHand(Clone)" in children
    GameObject handRoot = GameObject.Find("CreamConeHand(Clone)");
    if (handRoot == null)
    {
        Debug.LogError("Hand With Scooper not found.");
        return;
    }

    
    Transform coneParent = handRoot.transform.Find("cone/cream"+scoopcount);

    if (coneParent == null)
    {
        Debug.LogError("Thats the max amount of scoops you can put on the cone lil bro");
        return;
    }else{scoopcount++; }
    

    Renderer scoopRenderer = coneParent.GetComponent<Renderer>();
    if (scoopRenderer != null)
    {
        if (flavor.Contains("vanilla")) scoopRenderer.material = vanillaMat;
        else if (flavor.Contains("chocolate")) scoopRenderer.material = chocolateMat;
        else if (flavor.Contains("strawberry")) scoopRenderer.material = strawberryMat;
        else if (flavor.Contains("mango")) scoopRenderer.material = mangoMat;
        else if (flavor.Contains("blueberry")) scoopRenderer.material = blueberryMat;
    }
    else
    {
        Debug.LogError("Renderer not found on cream");
    }

    // scooped = true;
}



    public void PickUpCone(GameObject cones)
    {
        Vector3 spawnPosition = transform.position 
                                  + transform.right * 0.2f
                                  + transform.up * 1.1f
                                  + transform.forward * 0.40f; 
        Instantiate(cone, spawnPosition, transform.rotation, transform.parent);
        conePickedUp = true; 

    }
    public void GiveCone(GameObject customerObj)
        {
            CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();
            
            
            if (scooped == true)
            {
                customer.MoveToRegister(); // Call the method to make the customer leave
                Debug.Log("Gave cone to customer!");
                GameObject playerCamObj = GameObject.Find("PlayerCam");
                if (playerCamObj == null)
                {
                    Debug.LogError("PlayerCam not found in scene!");
                    return;
                }

            CustomerSpawner spawner = FindFirstObjectByType<CustomerSpawner>();
            if (spawner != null)
            {
                spawner.customerLine.RemoveAt(0);

                for (int i = 0; i < spawner.customerLine.Count; i++)
                {
                    spawner.customerLine[i].MoveToFront(spawner.queuePositions[i]);
                }
            }

                foreach (Transform child in playerCamObj.transform)
                {
                    if (child.CompareTag("Cone"))
                    {
                        Destroy(child.gameObject);
                        ResetScooped(); // Reset scooped to allow for new scooping
                        break;
                    }
                }
            }
        }
    
    public void RingUp(GameObject customerObj){
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();
        if (customer != null)
        {
            customer.Pay();
            MoneyDisplay moneyDisplay = FindObjectOfType<MoneyDisplay>();
            moneyDisplay.AddMoney(5); // Add money to the total
            // Sam add a command here to add the money to our total
            // samalama bim bam, bam, sam thank you ma'am, big lamb bam. if sam was a lamb, she would be a big lamb.
            Debug.Log("Customer has paid!");
        }
        else
        {
            Debug.LogError("CustomerMovement component not found on the object!");
        }

    }
            

    public void ResetScooped()
    {
        scooped = false;
        conePickedUp = false; 
        scoopcount = 1; 

    }


}
