using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour {
    public GameObject customerPrefab; 
    public Transform spawnPoint;
    public Transform targetPoint;
    public Transform exitPoint;
    public float timeBetweenSpawns = 20f;
    public int maxCustomers = 4;
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
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        CustomerMovement moveScript = newCustomer.GetComponent<CustomerMovement>();

        if (moveScript != null) {
            int index = customerLine.Count;
            if (index < queuePositions.Count) {
                moveScript.targetPoint = queuePositions[index];
            } else {
                Debug.LogWarning("Not enough queue positions for customer " + index);
            }

            customerLine.Add(moveScript);
        }
    }
}