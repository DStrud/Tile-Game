using System.Collections;
using UnityEngine;

public class SettlementTile : MonoBehaviour
{
    public float consumptionInterval = 5f; // Time in seconds between each resource consumption
    private float nextConsumptionTime = 0f;

    // Example consumption rates
    public int foodConsumptionRate = 2;
    public int woodConsumptionRate = 1;

    void Update()
    {
        if (Time.time >= nextConsumptionTime)
        {
            ConsumeResources();
            nextConsumptionTime = Time.time + consumptionInterval;
        }
    }

    void ConsumeResources()
    {
        // Check if there are enough resources available
        if (ResourceManager.Instance.GetResourceAmount(ResourceType.Food) >= foodConsumptionRate &&
            ResourceManager.Instance.GetResourceAmount(ResourceType.Wood) >= woodConsumptionRate)
        {
            // Consume resources
            ResourceManager.Instance.RemoveResource(ResourceType.Food, foodConsumptionRate);
            ResourceManager.Instance.RemoveResource(ResourceType.Wood, woodConsumptionRate);
            Debug.Log("Resources consumed by the settlement.");
        }
        else
        {
            Debug.Log("Not enough resources to consume.");
            // Handle the situation when resources are insufficient (e.g., impact on settlement)
        }
    }
}