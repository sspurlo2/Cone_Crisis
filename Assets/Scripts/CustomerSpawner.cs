using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour {
    public GameObject customerPrefab; 
    public Transform spawnPoint;
    public List<Transform> queuePositions; 
    public float timeBetweenSpawns = 20f;
    public int maxCustomers = 20;
    public List<CustomerMovement> customerLine = new List<CustomerMovement>();
    private int customersSpawned = 0;

    public void Start() {
        StartCoroutine(SpawnCustomers());
    }

    public IEnumerator SpawnCustomers() {
        while (customersSpawned < maxCustomers) {
            SpawnCustomer();
            customersSpawned++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void ResetSpawner()
    {
        StopAllCoroutines(); // In case previous spawns are still pending
        customersSpawned = 0;
        customerLine.Clear(); // Clear list to avoid leftover references
        StartCoroutine(SpawnCustomers());
    }


    void SpawnCustomer() {
        if (customerLine.Count >= queuePositions.Count) {
            Debug.LogWarning("No more queue spots left!");
            return;
        }

        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        CustomerMovement moveScript = newCustomer.GetComponent<CustomerMovement>();

        if (moveScript != null) {
            int nextSpotIndex = customerLine.Count;
            moveScript.targetPoint = queuePositions[nextSpotIndex];
            customerLine.Add(moveScript);
        }
    }
}
