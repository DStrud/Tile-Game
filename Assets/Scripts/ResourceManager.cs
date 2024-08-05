using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    //public Dictionary<ResourceType, Text> resourceUITexts = new Dictionary<ResourceType, Text>(); // UI Text references
    public Dictionary<ResourceType, TextMeshProUGUI> resourceUITexts = new Dictionary<ResourceType, TextMeshProUGUI>();


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeResourceUI();
    }

    void Start()
{
    // Initial resources
    int initialWood = 200;
    int initialStone = 100;
    int initialFood = 150;

    // Set the initial resources in the ResourceManager
    ResourceManager.Instance.AddResource(ResourceType.Wood, initialWood);
    ResourceManager.Instance.AddResource(ResourceType.Stone, initialStone);
    ResourceManager.Instance.AddResource(ResourceType.Food, initialFood);
}


void InitializeResourceUI()
{
    resourceUITexts[ResourceType.Stone] = GameObject.Find("StoneText").GetComponent<TextMeshProUGUI>();
    resourceUITexts[ResourceType.Wood] = GameObject.Find("WoodText").GetComponent<TextMeshProUGUI>();
    resourceUITexts[ResourceType.Food] = GameObject.Find("FoodText").GetComponent<TextMeshProUGUI>();
    // Repeat for other resources
}


    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
        UpdateResourceUI(type);
    }

void UpdateResourceUI(ResourceType type)
{
    if (resourceUITexts.ContainsKey(type) && resourceUITexts[type] != null)
    {
        resourceUITexts[type].text = resources[type].ToString();
    }
    else
    {
        Debug.LogError("Text element for " + type.ToString() + " is not initialized properly.");
    }
}

    public int GetResourceAmount(ResourceType type)
    {
        if (resources.ContainsKey(type))
            return resources[type];
        return 0;
    }

    public void RemoveResource(ResourceType type, int amount)
{
    if (resources.ContainsKey(type) && resources[type] >= amount)
    {
        resources[type] -= amount;
        UpdateResourceUI(type);
    }
}

}
