using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    public float accelerationRate = 0.1f; 
    public float maxAcceleration = 1.0f; 

    private float currentAcceleration = 0.0f; 

 
    public void IncreaseAcceleration()
    {
        if (currentAcceleration < maxAcceleration)
        {
            currentAcceleration += accelerationRate;
        }
    }


    public void ResetAcceleration()
    {
        currentAcceleration = 0.0f;
    }


    public float GetCurrentAcceleration()
    {
        return currentAcceleration;
    }

  
}