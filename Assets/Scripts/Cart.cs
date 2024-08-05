using UnityEngine;

public class Cart : MonoBehaviour
{
    public float moveSpeed = 5f;
    public ResourceType resourceType;
    public int resourceAmount;
    public Transform targetSettlement;

    void Update()
    {
        if (targetSettlement != null)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Move the cart towards the target settlement
        transform.position = Vector3.MoveTowards(transform.position, targetSettlement.position, moveSpeed * Time.deltaTime);

        // Check if the cart has reached the settlement
        if (Vector3.Distance(transform.position, targetSettlement.position) < 0.1f)
        {
            DeliverResources();
        }
    }

    void DeliverResources()
    {
        // Add resources to the global pool
        ResourceManager.Instance.AddResource(resourceType, resourceAmount);

        // Destroy the cart or return it for reuse
        Destroy(gameObject);
    }

    public void InitializeCart(ResourceType type, int amount, Transform settlement)
    {
        resourceType = type;
        resourceAmount = amount;
        targetSettlement = settlement;
    }
}
