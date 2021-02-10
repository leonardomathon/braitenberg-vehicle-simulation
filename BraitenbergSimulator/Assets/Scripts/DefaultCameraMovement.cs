using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCameraMovement : MonoBehaviour
{
    // Default camera 
    [SerializeField] private Camera cam;
    // The target to which to rotate around
    [SerializeField] private Transform target;

    // Minimal angle the camera is allowed to make whilst rotating around x-axis
    [SerializeField] private int minAngle = 20;

    // Maximum angle the camera is allowed to make whilst rotating around x-axis
    [SerializeField] private int maxAngle = 90;

    // Distance from target
    // TODO: Scroll to change this distance
    [SerializeField] private float distanceToTarget = 20;

    [SerializeField] private float zoomSpeed = 1;

    private Vector3 previousPosition;

    void Update()
    {
        // Get starting position as soon as mouse is pressed
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            // Mouseposition in viewport coordinates (0, 1)
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);

        }
        else if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            // Mouseposition in viewport coordinates (0, 1)
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);

            // Calculate what direction the mouse is moving to
            Vector3 direction = previousPosition - newPosition;

            // Rotation all the way from left to right should rotate cam 180 degrees
            float rotationY = -direction.x * 180;
            float rotationX = direction.y * 180;

            // Temporarily set camera position to target
            cam.transform.position = target.position;

            // Get the current x axis rotation
            Vector3 currentRotation = cam.transform.rotation.eulerAngles;

            // Only perform movement if allowed
            if (currentRotation.x + rotationX > minAngle && currentRotation.x + rotationX < maxAngle)
            {
                cam.transform.Rotate(new Vector3(1, 0, 0), rotationX);
            }
            else if (currentRotation.x + rotationX < minAngle)
            {
                // Reset angle to minAngle, so that the camera does not get stuck when changing parameter fields
                cam.transform.rotation = Quaternion.Euler(minAngle, currentRotation.y, currentRotation.z);
            }
            else if (currentRotation.x + rotationX > maxAngle)
            {
                // Reset angle to maxAngle, so that the camera does not get stuck when changing parameter fields
                cam.transform.rotation = Quaternion.Euler(maxAngle, currentRotation.y, currentRotation.z);
            }

            cam.transform.Rotate(new Vector3(0, 1, 0), rotationY, Space.World);

            // Translate camera back (undo cam.transform.position)
            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            // Update position
            previousPosition = newPosition;
        }
    }
}
