using UnityEngine;

public class Scooper : MonoBehaviour
{
    public Camera cam;
    public LayerMask iceCreamLayer;
    public LayerMask coneLayer;
    public GameObject cone;
    private bool scooped = false;
    private bool conePickedUp = false;
    public Material blueberryMat, chocolateMat, mangoMat, strawberryMat, vanillaMat;
    private int scoopcount = 1;
    private int price = 0;
    public float maxDistance = 3f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 1. Ice Cream Clicked
            if (Physics.Raycast(ray, out hit, maxDistance, iceCreamLayer))
            {
                Debug.Log("Clicked on " + hit.collider.name);

                // Inside the Ice Cream Clicked block
            if (!conePickedUp)
            {
                // Check if the tutorial is at step 1, or handle step 0 if needed
                if (TutorialManager.Instance.step == 1)
                    TutorialManager.Instance.ForceMessage("Click cone first!");
                else if (TutorialManager.Instance.step == 0)
                    TutorialManager.Instance.AdvanceStep();
            }
                else
                {
                    if (TutorialManager.Instance != null && TutorialManager.Instance.step == 2)
                        TutorialManager.Instance.AdvanceStep();

                    IceCreamSupply supply = hit.collider.GetComponentInParent<IceCreamSupply>();
                    if (supply != null)
                    {
                        if (supply.UseScoop())
                        {
                            SpawnCone(hit.collider.gameObject);
                            if (scoopcount > 2) price += 2;
                            scooped = true;
                        }
                        else
                        {
                            Debug.Log("No scoops left! Restock required.");
                        }
                    }
                }
            }

            // 2. Cone Clicked
            if (Physics.Raycast(ray, out hit, maxDistance, coneLayer))
            {
                Debug.Log("Clicked on " + hit.collider.name);
                if (!scooped)
                {
                    PickUpCone(hit.collider.gameObject);

                    if (TutorialManager.Instance.step == 1)
                        TutorialManager.Instance.AdvanceStep();
                }
            }

            // 3. Customer Clicked
            if (Physics.Raycast(ray, out hit, maxDistance, CustomerMovement.customerLayer))
            {
                Debug.Log("Clicked on customer " + hit.collider.name);
                CustomerMovement customer = hit.collider.GetComponent<CustomerMovement>();

                if (!scooped)
                {
                    RingUp(customer.gameObject);
                }

                if (customer != null)
                {
                    GiveCone(customer.gameObject);

                    if (TutorialManager.Instance.step == 3)
                        TutorialManager.Instance.AdvanceStep();
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

            if (flavor.Contains("vanilla")) {
                scoopRenderer.material = vanillaMat;
                cleanedFlavor = "Vanilla";
            }
            else if (flavor.Contains("chocolate")) {
                scoopRenderer.material = chocolateMat;
                cleanedFlavor = "Chocolate";
            }
            else if (flavor.Contains("strawberry")) {
                scoopRenderer.material = strawberryMat;
                cleanedFlavor = "Strawberry";
            }
            else if (flavor.Contains("mango")) {
                scoopRenderer.material = mangoMat;
                cleanedFlavor = "Mango";
            }
            else if (flavor.Contains("blueberry")) {
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
    }

    public void GiveCone(GameObject customerObj)
    {
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();

        if (scooped)
        {
            customer.MoveToRegister();
            Debug.Log("Gave cone to customer!");

            GameObject playerCamObj = GameObject.Find("PlayerCam");
            if (playerCamObj == null)
            {
                Debug.LogError("PlayerCam not found!");
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

            PlayerStack player = FindFirstObjectByType<PlayerStack>();
            if (player != null)
            {
                player.TrySubmitOrder();
            }
        }
    }
    public void RingUp(GameObject customerObj)
    {
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();
        if (customer != null)
        {
            customer.Pay();

            // Only add money if not in the tutorial
            if (!TutorialManager.Instance.isTutorial)
            {
                MoneyDisplay moneyDisplay = FindFirstObjectByType<MoneyDisplay>();
                moneyDisplay.AddMoney(5 + price);
            }

            price = 0;
            Debug.Log("Customer has paid!");

            // Advance tutorial step if on step 4
            if (TutorialManager.Instance.step == 4)
                TutorialManager.Instance.AdvanceStep();
        }
        else
        {
            Debug.LogError("CustomerMovement not found!");
        }
    }

    

    public void ResetScooped()
    {
        scooped = false;
        conePickedUp = false;
        scoopcount = 1;
    }
}
