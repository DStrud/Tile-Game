using UnityEngine;

public class ResourceTile : Tile // Assuming Tile is your base tile class
{
    public GameObject cartPrefab; // Assign the cart prefab in the inspector
    public ResourceType resourceType;
    public int productionRate;
    public int accumulatedResources = 5;
    public float dispatchInterval = 10f; // Time in seconds between cart dispatches
    public float timeSinceLastDispatch = 0f;

    void Update()
    {
        //AccumulateResources();
        timeSinceLastDispatch += Time.deltaTime; // Increment the timer

        if (timeSinceLastDispatch >= dispatchInterval)
        {
            DispatchCart();
            timeSinceLastDispatch = 0f; // Reset the timer
        }
    }

    //void AccumulateResources()
    //{
        // Accumulate resources over time
        // This could be time-based or event-based
        //accumulatedResources += productionRate; // Example: add resources each update
    //}

    void DispatchCart()
    {
            Transform closestSettlement = FindClosestSettlement();
            if (closestSettlement != null)
            {
                GameObject cartObject = Instantiate(cartPrefab, transform.position, Quaternion.identity);
                Cart cart = cartObject.GetComponent<Cart>();
                cart.InitializeCart(resourceType, accumulatedResources, closestSettlement);
            }
    }

    Transform FindClosestSettlement()
    {
        var allSettlements = SettlementManager.Instance.GetAllSettlements();
        Transform closestSettlement = null;
        float minDistance = float.MaxValue;

        foreach (Transform settlement in allSettlements)
        {
            float distance = Vector3.Distance(transform.position, settlement.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSettlement = settlement;
            }
        }

        return closestSettlement;
    }
}
