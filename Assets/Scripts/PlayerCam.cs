using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    // void Start()
    // {
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }
    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        
        xRotation = Mathf.Clamp(xRotation, -82f, 82f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
