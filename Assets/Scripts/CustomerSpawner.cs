using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour {
    public GameObject customerPrefab; 
    public Transform spawnPoint;
    public Transform targetPoint;
    public Transform exitPoint;
    public float timeBetweenSpawns = 20f;
    public int maxCustomers = 20;
    public List<CustomerMovement> customerLine = new List<CustomerMovement>();
    private int customersSpawned = 0;
    public List<Transform> queuePositions; 

    void Start() {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers() {
        while (customersSpawned < maxCustomers) {
            SpawnCustomer();
            customersSpawned++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    void SpawnCustomer() {
        if (customerLine.Count >= queuePositions.Count) {
            Debug.LogWarning("No more queue spots left!");
            return;
        }

        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        CustomerMovement moveScript = newCustomer.GetComponent<CustomerMovement>();
        
        if (moveScript != null) {
            // Assign the next available queue position
            int nextSpotIndex = customerLine.Count;
            moveScript.targetPoint = queuePositions[nextSpotIndex];
            customerLine.Add(moveScript); // Add to the line
            Debug.Log($"Spawned customer. Queue spot: {nextSpotIndex}");
        }
    }
}
