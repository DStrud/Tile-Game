using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20.0f;
    private Vector3 dragOrigin;
    public GridManager gridManager;

    void Update()
    {
        if (gridManager.isUIActive || EventSystem.current.IsPointerOverGameObject())
        {
            return; // Do not process camera movement if the UI is active
        }
        
        // On mouse button press, set the origin of the drag
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                dragOrigin = hit.point;
            }
        }

        // While the mouse button is held down, adjust the camera position
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 difference = dragOrigin - hit.point;
                difference.y = 0; // Ensure movement is only on the X/Z plane

                // Move the camera by the calculated difference
                transform.position += difference;
            }
        }
    }
}
