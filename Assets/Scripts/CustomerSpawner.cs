using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour {
    public GameObject customerPrefab; 
    public Transform spawnPoint;
    public Transform targetPoint;
    public Transform exitPoint;
    public float timeBetweenSpawns = 20f;
    public int maxCustomers = 5;
    public List<CustomerMovement> customerLine = new List<CustomerMovement>();
    private int customersSpawned = 0;

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
            Vector3 offset = new Vector3(0, 0, customerLine.Count * 1.5f);
            Vector3 adjustedTargetPos = targetPoint.position + offset;

            GameObject tempTarget = new GameObject("TempTarget");
            tempTarget.transform.position = adjustedTargetPos;

            moveScript.targetPoint = tempTarget.transform;
            moveScript.exitPoint = exitPoint;

            customerLine.Add(moveScript);
            Destroy(tempTarget, 30f);
        }
    }
}