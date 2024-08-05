using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    public GameObject tilePrefab; // The prefab for the tile you want to place
    private GameObject currentTile; // The current tile being placed

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            if (currentTile == null)
            {
                PlaceTile();
            }
            else
            {
                currentTile = null; // Deselect the tile
            }
        }

        if (currentTile != null)
        {
            MoveTileToCursor();
        }
    }

    void PlaceTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure the tile isn't placed with a z-offset

        currentTile = Instantiate(tilePrefab, mousePos, Quaternion.identity);
    }

    void MoveTileToCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure the tile moves along the x-y plane
        currentTile.transform.position = mousePos;
    }
}
