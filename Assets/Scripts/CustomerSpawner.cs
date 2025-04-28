using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour {
    public GameObject customerPrefab; 
    public Transform spawnPoint; // spawn point
    public Transform targetPoint; //target point (in front of counter)
    public Transform exitPoint; //exit spot
    public float timeBetweenSpawns = 20f; //10 seconds between customers
    public int maxCustomers = 5; //5 customer max rn
    private int customersSpawned = 0;

    private void Start() {
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
        moveScript.targetPoint = targetPoint;
        moveScript.exitPoint = exitPoint;
    }
}
