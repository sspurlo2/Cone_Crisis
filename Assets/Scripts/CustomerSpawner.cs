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
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        CustomerMovement moveScript = newCustomer.GetComponent<CustomerMovement>();

        if (moveScript != null) {
            int index = customerLine.Count;
            Transform target;

            if (index < queuePositions.Count) {
                target = queuePositions[index];
            } else {
                // Create a new waiting position further back
                Vector3 newPos = queuePositions[queuePositions.Count - 1].position + new Vector3(0, 0, (index - queuePositions.Count + 1) * 1.5f);
                GameObject tempTarget = new GameObject("ExtraQueueSpot_" + index);
                tempTarget.transform.position = newPos;
                target = tempTarget.transform;
            }

            moveScript.targetPoint = target;
            customerLine.Add(moveScript);
            Debug.Log($"Spawned customer #{index} at {target.position}");
        }
    }
}
