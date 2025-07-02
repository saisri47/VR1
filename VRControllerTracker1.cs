using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Tracks the movement of VR controllers and logs their positions
/// when they move significantly beyond a threshold.
/// </summary>
/// <remarks>
/// Code made by Saisri, email:saisribogapathi@gmail.com  
/// </remarks>
public class VRControllerTracker1 : MonoBehaviour
{
    private Vector3 lastLeftHandPosition;
    private Vector3 lastRightHandPosition;
    private float positionChangeThreshold = 0.5f; // Minimum movement to trigger a log

    /// <summary>
    /// Called at the start of the script.
    /// Initializes and logs the initial left hand position.
    /// </summary>
    
    void Start()
    {
        Debug.Log($"Last left hand position is {lastLeftHandPosition}");
    }

    /// <summary>
    /// Called once per frame.
    /// Continuously tracks the left controller's position.
    /// </summary>
    
    void Update()
    {
        TrackController(XRNode.LeftHand, ref lastLeftHandPosition);
        //TrackController(XRNode.RightHand, ref lastRightHandPosition);
    }

    /// <summary>
    /// Tracks the specified controller's position and logs it
    /// if it changes significantly from the last recorded position.
    /// </summary>
    
    void TrackController(XRNode node, ref Vector3 lastPosition)
    {
        InputDevice controller = InputDevices.GetDeviceAtXRNode(node);


        if (controller.isValid)
        {
            if (controller.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
            {
                // Only log if the position has changed significantly
                if (Vector3.Distance(position, lastPosition) > positionChangeThreshold)
                {
                    Debug.Log($"{node} Controller Position: {position}");
                    lastPosition = position; // Update the last known position
                }
            }
        }
        else
        {
            Debug.LogWarning($"{node} Controller not detected.");
        }
    }
}