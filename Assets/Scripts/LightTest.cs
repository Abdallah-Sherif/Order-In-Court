using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    public float toggleInterval = 2f; // Time interval for toggling in seconds
    public Light targetLight; // Reference to the Light component

    private bool isLightOn = true;
    private float timer;

    void Start()
    {
        if (targetLight == null)
        {
            // If the Light component is not assigned in the inspector, try to find it on the same GameObject
            targetLight = GetComponent<Light>();

            if (targetLight == null)
            {
                // If the Light component is still not found, log an error and disable the script
                Debug.LogError("Light component not found. Please assign the Light component to the script or attach it to the same GameObject.");
                enabled = false;
            }
        }
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to toggle
        if (timer >= toggleInterval)
        {
            // Reset the timer
            timer = 0f;

            // Toggle the light
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        // Toggle the state of the light
        isLightOn = !isLightOn;

        // Set the light intensity based on the state
        targetLight.intensity = isLightOn ? 40f : 0f;
    }
}
