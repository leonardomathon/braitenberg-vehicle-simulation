using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Default camera 
    [SerializeField] private Camera cam;
    // The target to which to rotate around
    [SerializeField] private Transform target;

    // The default target
    [SerializeField] private Transform defaultTarget;

    // Minimal angle the camera is allowed to make whilst rotating around x-axis
    [SerializeField] [Range(0, 90)] private int minAngle = 20;

    // Maximum angle the camera is allowed to make whilst rotating around x-axis
    [SerializeField] [Range(0, 90)] private int maxAngle = 90;

    // Distance from target
    [SerializeField] [Range(5, 15)] private float distanceToTarget = 20;

    // Minimum zoom distance
    [SerializeField] [Range(0, 90)] private int minZoomDistance = 5;

    // Maximum zoom distance
    [SerializeField] [Range(0, 90)] private int maxZoomDistance = 15;

    // Speed to zoom
    [SerializeField] [Range(1, 10)] private float zoomSpeed = 8;

    // Interpolation speed
    [SerializeField] private float interpolationSpeed = 0.05f;

    // Returns true if the camera is moving
    [SerializeField] private bool cameraIsMoving;

    // Returns true of the cameraIsMoving will be reset
    private bool resettingIsMoving;

    // Variables that locks the camera's inputs
    [SerializeField] private bool inputLocked = false;

    // If true, the camera will follow the target
    [SerializeField] private bool followTarget = true;

    // Returns true if the overview camera is enabled
    [SerializeField] private bool overviewCameraEnabled;

    // Previous position used to update the camera to its new position
    private Vector3 previousPosition;

    // Singleton pattern for CameraController
    #region singleton
    private static CameraController _instance;

    public static CameraController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    void Start()
    {
        target = defaultTarget;
    }

    void Update()
    {
        if (overviewCameraEnabled)
        {

            // Get the destination in terms of x and z
            Vector3 destinationPos = new Vector3(target.position.x, 20, target.position.z);

            // Interpolate to that destination
            Vector3 smoothPos = Vector3.Slerp(cam.transform.position, destinationPos, interpolationSpeed * Time.unscaledDeltaTime);

            // Calculate the relative position on one axis only  (vector cam-target direction)
            Vector3 relativePos = new Vector3(0, target.position.y - cam.transform.position.y, 0);

            // Calculate rotation
            Quaternion toRotation = Quaternion.LookRotation(relativePos, target.transform.up);

            // Interpolate rotation to make it smooth
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, toRotation, interpolationSpeed * Time.unscaledDeltaTime);

            // Move camera
            cam.transform.position = smoothPos;

            // Set distanceToTarget to avoid flickering on mouse input
            distanceToTarget = Vector3.Distance(cam.transform.position, target.position);

        }

        // If target moves, move camera with it
        if (followTarget && target.transform.hasChanged)
        {
            // Set to true, since we are moving the camera
            cameraIsMoving = true;

            if (cam.transform.rotation.eulerAngles.x < 85)
            {
                // Calculate the relative position (vector cam-target direction)
                Vector3 relativePos = target.position - cam.transform.position;

                // Calculate rotation
                Quaternion toRotation = Quaternion.LookRotation(relativePos);

                // Interpolate rotation to make it smooth
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, toRotation, interpolationSpeed * Time.unscaledDeltaTime);

                // Set distanceToTarget to avoid flickering on mouse input
                distanceToTarget = Vector3.Distance(cam.transform.position, target.position);
            }
            else
            {
                // Get the destination in terms of x and z
                Vector3 destinationPos = new Vector3(target.position.x, cam.transform.position.y, target.position.z);

                // Interpolate to that destination
                Vector3 smoothPos = Vector3.Slerp(cam.transform.position, destinationPos, interpolationSpeed * Time.unscaledDeltaTime);

                // Move camera
                cam.transform.position = smoothPos;
            }

            if (cam.transform.position == target.position) target.transform.hasChanged = false;
        }

        if (!inputLocked)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                // Set to true, since we are moving the camera
                cameraIsMoving = true;

                // Set the camera position to 0.0
                cam.transform.position = target.position;

                // Change distanceToTarget upon scrolling and keep distance between 5 and 15
                distanceToTarget = Mathf.Clamp(distanceToTarget - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoomDistance, maxZoomDistance);

                // Transform 
                cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
            }

            // Get starting position as soon as mouse is pressed
            if (Input.GetMouseButtonDown(2))
            {
                // Mouseposition in viewport coordinates (0, 1)
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);

            }
            else if (Input.GetMouseButton(2))
            {
                // Set to true, since we are moving the camera
                cameraIsMoving = true;

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


            // Only reset is moving variable if no invoke is called
            if (!resettingIsMoving)
            {
                StartCoroutine(ResetIsMoving(2f));
                resettingIsMoving = true;
            }
        }
    }

    public void SetTarget(GameObject obj)
    {
        target = obj.transform;
        target.transform.hasChanged = true;
    }

    public void SetTarget(Transform obj)
    {
        target = obj;
        target.transform.hasChanged = true;
    }

    public void ResetTarget()
    {
        target = defaultTarget;
        target.transform.hasChanged = true;
    }

    public void LockCamera()
    {
        inputLocked = true;
    }

    public void UnlockCamera()
    {
        inputLocked = false;
    }

    public void FollowTarget()
    {
        followTarget = true;
    }

    public void UnfollowTarget()
    {
        followTarget = false;
    }

    public void EnableOverviewCamera()
    {
        overviewCameraEnabled = true;
        SetTarget(defaultTarget);
        LockCamera();
    }

    public void DisableOverviewCamera(GameObject target)
    {
        overviewCameraEnabled = false;
        SetTarget(target);
        UnlockCamera();
    }

    public bool CameraIsMoving()
    {
        return cameraIsMoving;
    }

    public Camera GetCurrentCam()
    {
        return cam;
    }

    private IEnumerator ResetIsMoving(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        cameraIsMoving = false;
        resettingIsMoving = false;
    }
}
