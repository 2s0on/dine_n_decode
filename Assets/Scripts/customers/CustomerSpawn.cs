using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject customerPrefab; // assign your Customer prefab
    public Transform[] spawnPoints; // UI positions where customers appear
    public float spawnInterval = 5f; // time between spawns
    public int maxCustomers = 3; // max number of customers at once

    private int currentCustomers = 0;

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (currentCustomers < maxCustomers)
            {
                SpawnCustomer();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCustomer()
    {
        if (spawnPoints.Length == 0 || customerPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject customerGO = Instantiate(customerPrefab, spawnPoint, false); // false keeps UI scaling
        currentCustomers++;

        // Optional: let the customer tell the spawner when they leave
        Customer customer = customerGO.GetComponent<Customer>();
        if (customer != null)
            customer.onCustomerLeave += () => currentCustomers--;
    }
}