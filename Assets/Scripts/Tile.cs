using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Tile : MonoBehaviour
{
    public bool IsSelected { get; private set; }
    private Renderer childRenderer;
    public GameObject selector;

    void Start()
    {
        // This will get the Renderer component of the first child that has one
        childRenderer = GetComponentInChildren<Renderer>();
    }

    public void Select()
    {
        IsSelected = true;
        // Change the child tile appearance to indicate selection
        //childRenderer.material.color = Color.red;
        selector.SetActive(true);
    }

    public void Deselect()
    {
        IsSelected = false;
        // Revert the child tile appearance
        //childRenderer.material.color = Color.white;
        selector.SetActive(false);
    }

public enum TileType
{
    Plain,
    Forest,
    Water,
    Rock,
    Feilds,
    Settlement,
    LoggingCamp,
    RockQuarry,
    CoalMine,
    IronMine,
    GoldMine,

    // add other types as needed
}
public TileType type; // Add this line in the Tile class

}
