using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour {
    [Header("Customer Prefabs Per Day")]
    public List<GameObject> customerPrefabsPerDay; // Set in inspector (1 per day)

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public List<Transform> queuePositions;
    public float timeBetweenSpawns = 20f;
    public int maxCustomers = 20;

    [HideInInspector]
    public List<CustomerMovement> customerLine = new List<CustomerMovement>();

    private int customersSpawned = 0;
    private int currentDay = 1;

    public void Start() {
        StartCoroutine(SpawnCustomers());
    }

    public void SetDay(int day) {
        currentDay = day;
    }

    public IEnumerator SpawnCustomers() {
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

        GameObject prefabToSpawn = (currentDay - 1 < customerPrefabsPerDay.Count) 
            ? customerPrefabsPerDay[currentDay - 1] 
            : customerPrefabsPerDay[0]; // fallback

        GameObject newCustomer = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        CustomerMovement moveScript = newCustomer.GetComponent<CustomerMovement>();

        if (moveScript != null) {
            int nextSpotIndex = customerLine.Count;
            moveScript.targetPoint = queuePositions[nextSpotIndex];
            customerLine.Add(moveScript);
        }
    }

    public void ResetSpawner() {
        StopAllCoroutines();
        customersSpawned = 0;
        customerLine.Clear();
        StartCoroutine(SpawnCustomers());
    }
}