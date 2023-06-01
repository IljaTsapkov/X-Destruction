using UnityEngine;

public class FollowCameraZ : MonoBehaviour
{
    public Transform cameraTransform;
    public float zOffset = 1.0f;
    public float objectHeight = 1.0f;

    private Vector3 offset;

    void Start()
    {
        // Calculate the initial offset
        offset = new Vector3(0f, objectHeight, -zOffset);
    }

    void LateUpdate()
    {
        // Set the position of the object behind the camera on the Z axis only
        Vector3 cameraPos = cameraTransform.position;
        Vector3 objectPos = cameraPos - cameraTransform.forward * zOffset;
        objectPos.y = cameraPos.y + objectHeight;
        transform.position = objectPos;

        // Rotate the object to face the camera on the Y axis only
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0;
        if (directionToCamera != Vector3.zero) // Avoid errors when the direction to camera is zero
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
            transform.rotation = targetRotation;
        }
    }
}
