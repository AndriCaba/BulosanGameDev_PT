using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target; // The target (e.g., the player) the camera should follow
    public float smoothSpeed = 0.125f; // Smoothness of the camera's movement
    public Vector3 offset; // Offset from the target (e.g., distance behind the player)

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position: target's position + the offset
            Vector3 desiredPosition = target.position + offset;

            // We only want to move the camera on the X and Y axes in a 2D game
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position to the smoothed position
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}

