using UnityEngine;
using System.Collections.Generic;

public class SettlementManager : MonoBehaviour
{
    public static SettlementManager Instance { get; private set; }
    public List<Transform> settlementTiles = new List<Transform>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterSettlement(Transform settlementTile)
    {
        if (!settlementTiles.Contains(settlementTile))
            settlementTiles.Add(settlementTile);
    }

    public void UnregisterSettlement(Transform settlementTile)
    {
        settlementTiles.Remove(settlementTile);
    }

    // Make sure this method is defined and public
    public List<Transform> GetAllSettlements()
    {
        return settlementTiles;
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

    if (closestSettlement == null)
    {
        Debug.Log("No settlement found");
    }
    else
    {
        Debug.Log("Closest settlement is: " + closestSettlement.gameObject.name);
    }

    return closestSettlement;
}
}
