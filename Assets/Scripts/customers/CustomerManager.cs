using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject customerPrefab; // customer prefab to spawn
    public Transform[] spawnPoints;   // positions where customers can appear
    public int maxCustomers = 3;      // max number of customers at the same time

    [Header("Spawn Timing")]
    public float minSpawnDelay = 1f;  // minimum delay between spawns
    public float maxSpawnDelay = 4f;  // maximum delay between spawns

    private GameObject[] activeCustomers; // array to track current active customers

    private void Start()
    {
        activeCustomers = new GameObject[spawnPoints.Length]; // initialize array

        // spawn initial customers with a small random delay
        StartCoroutine(SpawnInitialCustomers());
    }

    private IEnumerator SpawnInitialCustomers()
    {
        for (int i = 0; i < maxCustomers; i++)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay); // random delay
            yield return new WaitForSeconds(delay); // wait before spawning
            SpawnCustomerAt(i); // spawn customer at index
        }
    }

    private void SpawnCustomerAt(int index)
    {
        if (activeCustomers[index] == null) // only spawn if slot is empty
        {
            // instantiate customer prefab at spawn point, parented to manager
            GameObject newCustomer = Instantiate(customerPrefab, spawnPoints[index].position, Quaternion.identity, transform);
            newCustomer.transform.localScale = Vector3.one; // ensure correct scale
            activeCustomers[index] = newCustomer; // store in active customers array

            // get customer script and links to onCustomerLeave event
            Customer custScript = newCustomer.GetComponent<Customer>();
            custScript.onCustomerLeave += () => HandleCustomerLeave(index); // handle leaving
        }
    }

    private void HandleCustomerLeave(int index)
    {
        activeCustomers[index] = null; // mark slot as empty
        StartCoroutine(RespawnCustomerAfterDelay(index)); // respawn after delay
    }

    private IEnumerator RespawnCustomerAfterDelay(int index)
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay); // random respawn delay
        yield return new WaitForSeconds(delay); // wait
        SpawnCustomerAt(index); // respawn customer at index
    }
}
