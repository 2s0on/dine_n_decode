using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject customerPrefab; // customer prefab
    public Transform[] spawnPoints;   // the points where customers can spawn
    public int maxCustomers = 3;      // maximum number of customers allowed at once

    [Header("Spawn Timing")]
    public float minSpawnDelay = 1f;  // min max delay of customer spawn
    public float maxSpawnDelay = 4f;  

    private GameObject[] activeCustomers;

    private void Start()
    {
        activeCustomers = new GameObject[spawnPoints.Length];

        // spawn initial customers with delays
        StartCoroutine(SpawnInitialCustomers());
    }

    private IEnumerator SpawnInitialCustomers()
    {
        for (int i = 0; i < maxCustomers; i++)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
            SpawnCustomerAt(i);
        }
    }

    private void SpawnCustomerAt(int index)
    {
        if (activeCustomers[index] == null)
        {
            GameObject newCustomer = Instantiate(customerPrefab, spawnPoints[index].position, Quaternion.identity, transform);
            newCustomer.transform.localScale = Vector3.one; // keep scale correct
            activeCustomers[index] = newCustomer;

            // customer script reference to handle leaving
            Customer custScript = newCustomer.GetComponent<Customer>();
            custScript.onCustomerLeave += () => HandleCustomerLeave(index);
        }
    }

    private void HandleCustomerLeave(int index)
    {
        activeCustomers[index] = null;
        StartCoroutine(RespawnCustomerAfterDelay(index));
    }

    private IEnumerator RespawnCustomerAfterDelay(int index)
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(delay);
        SpawnCustomerAt(index);
    }
}
