using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundaries : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // Reference to the player
    public Vector2 offset;   // Offset from the player (camera position relative to player)
    public float followSpeed = 5f; // Speed of following the player

    [Header("Bounds Settings")]
    public bool useBounds = true; // Enable or disable camera boundaries
    public Vector2 minBounds; // Min X and Y for the camera
    public Vector2 maxBounds; // Max X and Y for the camera

    [Header("Cinemachine Settings")]
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    private CinemachineTransposer transposer; // The component to modify the camera's position

    void Start()
    {
        // Get the Transposer component from the CinemachineVirtualCamera
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer == null)
        {
            Debug.LogError("Cinemachine Transposer not found on Virtual Camera.");
        }
    }

    void Update()
    {
        if (player == null || transposer == null) return;

        // Calculate the target position of the camera with the offset
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transposer.m_FollowOffset.z);

        // Apply the camera boundaries if enabled
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }

        // Smoothly move the camera's position to follow the player with optional boundaries
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetPosition - player.position, followSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            // Draw a rectangle representing the camera's movement boundaries
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(minBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, minBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0), new Vector3(minBounds.x, minBounds.y, 0));
        }
    }
}
