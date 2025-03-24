using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Camera mainCamera;

    //this script for look at canvas to camera

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}
