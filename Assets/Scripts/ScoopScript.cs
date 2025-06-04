using UnityEngine;

public class Scooper : MonoBehaviour
{
    public Camera cam;
    public LayerMask iceCreamLayer;
    public LayerMask coneLayer;
    public GameObject cone;

    private bool scooped = false;
    private bool conePickedUp = false;

    public Material blueberryMat;
    public Material chocolateMat;
    public Material mangoMat;
    public Material strawberryMat;
    public Material vanillaMat;
    public Material MintChocolateMat;

    private int scoopcount = 1;
    private int price = 0;

    public float maxDistance = 3f;
    public AudioClip caChingClip;
    public AudioClip scoopSound;     // New scoop sound clip
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("Scooper is missing AudioSource!");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, iceCreamLayer))
            {
                Debug.Log("Clicked on " + hit.collider.name);
                if (conePickedUp)
                {
                    IceCreamSupply supply = hit.collider.GetComponentInParent<IceCreamSupply>();

                    if (supply != null)
                    {
                        if (supply.UseScoop())
                        {
                            SpawnCone(hit.collider.gameObject);
                            if (scoopcount > 2) price += 2;
                            scooped = true;

                            // Play scoop sound
                            if (scoopSound != null && audioSource != null)
                                audioSource.PlayOneShot(scoopSound);

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
                }
                else
                {
                    Debug.Log("Cannot scoop: Already scooped or no cone picked up.");
                }
            }

            if (Physics.Raycast(ray, out hit, maxDistance, coneLayer))
            {
                Debug.Log("Clicked on " + hit.collider.name);
                if (!scooped)
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
                        timer.StopTimer();
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
        else scoopcount++;

        Renderer scoopRenderer = coneParent.GetComponent<Renderer>();
        if (scoopRenderer != null)
        {
            string cleanedFlavor = "";

            if (flavor.Contains("vanilla")) { scoopRenderer.material = vanillaMat; cleanedFlavor = "Vanilla"; }
            else if (flavor.Contains("chocolate")) { scoopRenderer.material = chocolateMat; cleanedFlavor = "Chocolate"; }
            else if (flavor.Contains("strawberry")) { scoopRenderer.material = strawberryMat; cleanedFlavor = "Strawberry"; }
            else if (flavor.Contains("mango")) { scoopRenderer.material = mangoMat; cleanedFlavor = "Mango"; }
            else if (flavor.Contains("blueberry")) { scoopRenderer.material = blueberryMat; cleanedFlavor = "Blueberry"; }
            else if (flavor.Contains("mint")) { scoopRenderer.material = blueberryMat; cleanedFlavor = "Blueberry"; }

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
            TutorialManager.Instance.AdvanceStep();
    }

    public void GiveCone(GameObject customerObj)
    {
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();

        if (scooped)
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

            PlayerStack stack = FindFirstObjectByType<PlayerStack>();
            CustomerOrder order = customerObj.GetComponentInChildren<CustomerOrder>();
            Debug.Log("Checking stack and order...");
            Debug.Log("stack is " + (stack == null ? "null" : "FOUND"));
            Debug.Log("order is " + (order == null ? "null" : "FOUND"));

            if (stack != null && order != null)
            {
                stack.SetCurrentOrder(order);
                Debug.Log("Set current order on PlayerStack.");
                Debug.Log("Calling TrySubmitOrder from Scooper!");
                stack.TrySubmitOrder();
            }
            else
            {
                if (stack == null)
                    Debug.LogWarning(" Could not find PlayerStack.");
                if (order == null)
                    Debug.LogWarning(" Could not find CustomerOrder in children of customerObj.");
            }

            if (TutorialManager.Instance != null && TutorialManager.Instance.step == 3)
                TutorialManager.Instance.AdvanceStep();
        }
    }



    public void RingUp(GameObject customerObj)
    {
        CustomerMovement customer = customerObj.GetComponent<CustomerMovement>();
        if (customer != null)
        {
            customer.Pay();

            if (TutorialManager.Instance == null || TutorialManager.Instance.step > 4)
            {
                MoneyDisplay moneyDisplay = FindFirstObjectByType<MoneyDisplay>();
                if (moneyDisplay != null)
                    moneyDisplay.AddMoney(5 + price);
                else
                    Debug.LogWarning("MoneyDisplay not found — skipping money logic in tutorial.");
            }

            // Play cash register sound
            if (caChingClip != null && audioSource != null)
                audioSource.PlayOneShot(caChingClip);

            price = 0;
            Debug.Log("Customer has paid!");

            if (TutorialManager.Instance != null && TutorialManager.Instance.step == 4)
                TutorialManager.Instance.AdvanceStep();
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
