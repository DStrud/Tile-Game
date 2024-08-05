using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // Assign your tile prefab in the inspector
    public int width = 10;        // Width of the grid
    public int height = 10;       // Height of the grid
    public float elevationScale = 1.0f; // Scale of the elevation
    public float tileSize = 1.0f; // Size of each tile
    private Tile selectedTile;
    public GameObject settlementTilePrefab;
    public GameObject LoggingCampTilePrefab;
    public GameObject FeildsTilePrefab;
    public GameObject StoneQuaryTilePrefab;
    public Canvas uiCanvas; // Assign your world space canvas in the inspector
    public float canvasHeightAboveTile = 2.0f; // Height above the tile to place the canvas
    public GameObject uiButton; // Assign your button or UI panel in the inspector
    public Camera mainCamera;   // Assign the main camera in the inspector
    public Vector3 uiOffset = new Vector3(0, 30, 0); // Offset to adjust the UI position over the tile
    public GameObject landTilePrefab; // Assign your land tile prefab
    public GameObject seaTilePrefab;  // Assign your sea tile prefab
    public GameObject forestTilePrefab; // Assign your forest tile prefab in the inspector
    public GameObject stoneTilePrefab; // Assign your stone tile prefab in the inspector
    public float forestNoiseScale = 0.1f; // Scale for forest Perlin noise
    public float stoneNoiseScale = 0.1f;  // Scale for stone Perlin noise
    public float stoneThreshold = 0.4f;   // Threshold for determining if a stone should be placed
    public float forestThreshold = 0.6f; // Threshold for determining if a forest should be placed
    public float noiseScale = 0.1f;   // Scale of the Perlin noise
    public bool isUIActive = false;


    void Start()
    {
        GenerateGrid();
        uiButton.SetActive(false);
    }

void GenerateGrid()
{
    for (int x = 0; x < width; x++)
    {
        for (int z = 0; z < height; z++)
        {
            // Determine land, sea, forest, or stone
            float landSeaPerlin = Mathf.PerlinNoise(x * noiseScale, z * noiseScale);
            float forestPerlin = Mathf.PerlinNoise(x * forestNoiseScale, z * forestNoiseScale);
            // Optionally, add another Perlin noise layer for stone or use existing values with different thresholds
            float stonePerlin = Mathf.PerlinNoise(x * stoneNoiseScale, z * stoneNoiseScale); // Define stoneNoiseScale if using a separate noise scale for stone

            GameObject tileToUse;

            // Modify this logic based on how you want stone tiles to be distributed
            if (landSeaPerlin > 0.5f)
            {
                if (forestPerlin > forestThreshold)
                {
                    // It's forest
                    tileToUse = forestTilePrefab;
                }
                else if (landSeaPerlin > stoneThreshold) // Define stoneThreshold where stone tiles should start appearing
                {
                    // It's stone
                    tileToUse = stoneTilePrefab; // Define stoneTilePrefab similar to your other prefabs
                }
                else
                {
                    // It's land
                    tileToUse = landTilePrefab;
                }
            }
            else
            {
                // It's sea
                tileToUse = seaTilePrefab;
            }

            // Calculate elevation, potentially modify for stone tiles if you want them to have distinct elevation
            float elevation = (landSeaPerlin - 0.5f) * elevationScale; // Adjust elevationScale to control the height difference
            if (landSeaPerlin <= 0.5f) elevation = 0; // Keep sea level flat, or adjust as needed

            GameObject newTile = Instantiate(tileToUse, new Vector3(x * tileSize, elevation, z * tileSize), Quaternion.identity);
            newTile.transform.parent = this.transform;
        }
    }
}




    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the click is over a UI element
            if (!EventSystem.current.IsPointerOverGameObject()) // This checks if the mouse is over a UI element
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Tile>() != null)
                    {
                        if (selectedTile != null)
                        {
                        selectedTile.Deselect();
                        }
                    selectedTile = hit.collider.gameObject.GetComponent<Tile>();
                    selectedTile.Select();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedTile != null) // Right mouse button
        {
            selectedTile.Deselect();
            selectedTile = null;
        }

        if (selectedTile != null)
        {
            uiButton.SetActive(true);
            isUIActive = true;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(selectedTile.transform.position + new Vector3(0, 1, 0));
            uiButton.transform.position = screenPosition + uiOffset;
        }
        else
        {
            uiButton.SetActive(false);
            isUIActive = false;
        }

    }


    public void SwapToSettlementTile()
{
    // Building cost
    int woodCost = 100;
    int stoneCost = 50;
    int foodCost = 50;

    // Check if the player has enough resources
    if (ResourceManager.Instance.GetResourceAmount(ResourceType.Wood) >= woodCost &&
        ResourceManager.Instance.GetResourceAmount(ResourceType.Stone) >= stoneCost &&
        ResourceManager.Instance.GetResourceAmount(ResourceType.Food) >= foodCost)
    {
        if (selectedTile != null)
        {
            Vector3 position = selectedTile.transform.position;
            Destroy(selectedTile.gameObject); // Destroy the old tile

            GameObject newTile = Instantiate(settlementTilePrefab, position, Quaternion.identity);
            newTile.transform.parent = this.transform;
            selectedTile = newTile.GetComponent<Tile>(); // Update the selected tile reference
            SettlementManager.Instance.RegisterSettlement(newTile.transform);

            // Deduct the resources cost
            ResourceManager.Instance.RemoveResource(ResourceType.Wood, woodCost);
            ResourceManager.Instance.RemoveResource(ResourceType.Stone, stoneCost);
            ResourceManager.Instance.RemoveResource(ResourceType.Food, foodCost);
        }
    }
    else
    {
        Debug.Log("Not enough resources to build the settlement.");
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


public void SwapToLoggingCampTile()
{
    // Define resource costs
    int woodCost = 50; // Example cost
    int stoneCost = 30; // Example cost

    // Check if the player has enough resources
    if (ResourceManager.Instance.GetResourceAmount(ResourceType.Wood) >= woodCost &&
        ResourceManager.Instance.GetResourceAmount(ResourceType.Stone) >= stoneCost)
    {
        if (selectedTile != null)
        {
            Vector3 position = selectedTile.transform.position;
            Destroy(selectedTile.gameObject); // Destroy the old tile

            GameObject newTile = Instantiate(LoggingCampTilePrefab, position, Quaternion.identity);
            newTile.transform.parent = this.transform;
            selectedTile = newTile.GetComponent<Tile>(); // Update the selected tile reference

            // Deduct the resource costs
            ResourceManager.Instance.RemoveResource(ResourceType.Wood, woodCost);
            ResourceManager.Instance.RemoveResource(ResourceType.Stone, stoneCost);
        }
    }
    else
    {
        Debug.Log("Not enough resources to build the Logging Camp.");
    }
}


public void SwapToFieldsTile()
{
    int woodCost = 30; // Example cost
    int foodCost = 20; // Example cost

    if (ResourceManager.Instance.GetResourceAmount(ResourceType.Wood) >= woodCost &&
        ResourceManager.Instance.GetResourceAmount(ResourceType.Food) >= foodCost)
    {
        if (selectedTile != null)
        {
            Vector3 position = selectedTile.transform.position;
            Destroy(selectedTile.gameObject);

            GameObject newTile = Instantiate(FeildsTilePrefab, position, Quaternion.identity);
            newTile.transform.parent = this.transform;
            selectedTile = newTile.GetComponent<Tile>();

            ResourceManager.Instance.RemoveResource(ResourceType.Wood, woodCost);
            ResourceManager.Instance.RemoveResource(ResourceType.Food, foodCost);
        }
    }
    else
    {
        Debug.Log("Not enough resources to build the Fields.");
    }
}

public void SwapToStoneQuarryTile()
{
    int woodCost = 40; // Example cost
    int stoneCost = 20; // Example cost

    if (ResourceManager.Instance.GetResourceAmount(ResourceType.Wood) >= woodCost &&
        ResourceManager.Instance.GetResourceAmount(ResourceType.Stone) >= stoneCost)
    {
        if (selectedTile != null)
        {
            Vector3 position = selectedTile.transform.position;
            Destroy(selectedTile.gameObject);

            GameObject newTile = Instantiate(StoneQuaryTilePrefab, position, Quaternion.identity);
            newTile.transform.parent = this.transform;
            selectedTile = newTile.GetComponent<Tile>();

            ResourceManager.Instance.RemoveResource(ResourceType.Wood, woodCost);
            ResourceManager.Instance.RemoveResource(ResourceType.Stone, stoneCost);
        }
    }
    else
    {
        Debug.Log("Not enough resources to build the Stone Quarry.");
    }
}

}