using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeaponHolder : MonoBehaviour
{
    public Camera attachedCamera;

    private void Start()
    {
        if (attachedCamera == null)
        {
            // If no camera is assigned, use the main camera
            attachedCamera = Camera.main;
        }

        if (attachedCamera == null)
        {
            Debug.LogError("No camera assigned and no main camera found in the scene!");
        }
    }

    private void Update()
    {
        if (attachedCamera != null)
        {
            // Make the object always face the direction the camera is looking
            Vector3 lookAtPosition = transform.position + attachedCamera.transform.forward;
            transform.LookAt(lookAtPosition);
                        transform.position = attachedCamera.transform.position;
            transform.rotation = attachedCamera.transform.rotation;
        }
    }
}
