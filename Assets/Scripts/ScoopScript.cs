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
    public Material MintChocolateMat;
    private int scoopcount = 1;
    private int price = 0; // Base price for a cone

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
                    IceCreamSupply supply = hit.collider.GetComponentInParent<IceCreamSupply>();

                    if (supply != null)
                    {
                        if (supply.UseScoop())
                        {
                            SpawnCone(hit.collider.gameObject); // Allow scooping
                            if (scoopcount > 2) { price += 2; }
                            scooped = true; // Set scooped to true

                            if (TutorialManager.Instance != null && TutorialManager.Instance.step == 2)
                            {
                                Debug.Log("Advancing tutorial from step 2 to 3");
                                TutorialManager.Instance.AdvanceStep();
                            }
                        }
                        else
                        {
                            Debug.Log("No scoops left! Restock required.");
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
                if (scooped == false)
                {
                    PickUpCone(hit.collider.gameObject);
                }
            }

            if (Physics.Raycast(ray, out hit, maxDistance, CustomerMovement.customerLayer))
            {
                Debug.Log("Clicked on customer " + hit.collider.name);
                CustomerMovement customer = hit.collider.GetComponent<CustomerMovement>();
                if (!scooped && customer.AtRegister())
                {
                    RingUp(customer.gameObject);
                }

                if (customer != null)
                {
                    if (!scooped)
                    {
                        customer.WalkOut();
                        FindObjectOfType<StarRatingDisplay>().IncreaseRating(-.5f);
                    }
                    else
                    {
                        GiveCone(customer.gameObject);
                        WorldSpaceTimer timer = FindFirstObjectByType<WorldSpaceTimer>();
                        timer.StopTimer(); // stop timer when giving cone instead of when they walk out
                    }
                }
            }
        }
    }

    void SpawnCone(GameObject tub)
    {
        string flavor = tub.name.ToLower();
        GameObject handRoot = GameObject.Find("CreamConeHand(Clone)");
        if (handRoot == null)
        {
            Debug.LogError("Hand With Scooper not found.");
            return;
        }

        Transform coneParent = handRoot.transform.Find("cone/cream" + scoopcount);

        if (coneParent == null)
        {
            Debug.LogError("That's the max amount of scoops you can put on the cone, lil bro");
            return;
        }
        else
        {
            scoopcount++;
        }

        Renderer scoopRenderer = coneParent.GetComponent<Renderer>();
        if (scoopRenderer != null)
        {
            string cleanedFlavor = "";

            if (flavor.Contains("vanilla"))
            {
                scoopRenderer.material = vanillaMat;
                cleanedFlavor = "Vanilla";
            }
            else if (flavor.Contains("chocolate"))
            {
                scoopRenderer.material = chocolateMat;
                cleanedFlavor = "Chocolate";
            }
            else if (flavor.Contains("strawberry"))
            {
                scoopRenderer.material = strawberryMat;
                cleanedFlavor = "Strawberry";
            }
            else if (flavor.Contains("mango"))
            {
                scoopRenderer.material = mangoMat;
                cleanedFlavor = "Mango";
            }
            else if (flavor.Contains("blueberry"))
            {
                scoopRenderer.material = blueberryMat;
                cleanedFlavor = "Blueberry";
            }
            else if (flavor.Contains("mint"))
            {
                scoopRenderer.material = blueberryMat;
                cleanedFlavor = "Blueberry";
            }

            if (!string.IsNullOrEmpty(cleanedFlavor))
            {
                PlayerStack player = FindFirstObjectByType<PlayerStack>();
                if (player != null)
                {
                    player.AddFlavor(cleanedFlavor);
                    Debug.Log($"Added flavor to player stack: {cleanedFlavor}");
                }
            }
        }
        else
        {
            Debug.LogError("Renderer not found on cream");
        }
    }



    public void PickUpCone(GameObject cones)
    {
        Vector3 spawnPosition = transform.position
                              + transform.right * 0.2f
                              + transform.up * 1.1f
                              + transform.forward * 0.40f;
        Instantiate(cone, spawnPosition, transform.rotation, transform.parent);
        conePickedUp = true;

        if (TutorialManager.Instance != null && TutorialManager.Instance.step == 1)
        {
            TutorialManager.Instance.AdvanceStep();
        }
    }

    public void GiveCone(GameObject customerObj)
    {
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();

        if (scooped == true)
        {
            Debug.Log("GiveCone() reached. Scooped is true.");

            customer.MoveToRegister();
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
                    ResetScooped();
                    break;
                }
            }
            if (TutorialManager.Instance != null && TutorialManager.Instance.step == 3)
            {
                Debug.Log("Advancing tutorial from step 3 to 4 (gave cone)");
                TutorialManager.Instance.AdvanceStep();
            }
        }
    }


    public void RingUp(GameObject customerObj)
    {
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();
        if (customer != null)
        {
            customer.Pay();

            // Only add money if we're not in the tutorial
            if (TutorialManager.Instance == null || TutorialManager.Instance.step > 4)
            {
                MoneyDisplay moneyDisplay = FindFirstObjectByType<MoneyDisplay>();
                if (moneyDisplay != null)
                {
                    moneyDisplay.AddMoney(5 + price);
                }
                else
                {
                    Debug.LogWarning("MoneyDisplay not found — skipping money logic in tutorial.");
                }
            }

            price = 0;
            Debug.Log("Customer has paid!");

            if (TutorialManager.Instance != null && TutorialManager.Instance.step == 4)
            {
                Debug.Log("Advancing tutorial from step 4 to 5 (ringed up)");
                TutorialManager.Instance.AdvanceStep();
            }
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