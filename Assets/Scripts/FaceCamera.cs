using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Find and store a reference to the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        // Rotate the canvas to face the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}
