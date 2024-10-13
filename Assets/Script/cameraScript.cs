using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform player, look;

    // Update is called once per frame
    void Update()
    {
        // Look at the target
        transform.LookAt(look.position);

        // Get the current rotation in Euler angles
        Vector3 rotation = transform.eulerAngles;

        // Ensure the X rotation doesn't go below -20 degrees (converting for Unity's 0-360 rotation)
        if (rotation.x > 180) rotation.x -= 360; // Convert range from [0, 360] to [-180, 180]
        rotation.x = Mathf.Clamp(rotation.x, -15f, 90f); // Clamp between -20 and 90 (for example)

        // Apply the clamped rotation
        transform.eulerAngles = rotation;
    }
}
