using UnityEngine;
using System;

/// <summary>
/// Controls the behavior of the ActionLog sub-button for View in a VR environment using physics raycasting.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapthi@gmail.com
/// </remarks>
public class ActionLogVRRaycast : MonoBehaviour
{
    [SerializeField] private string mainButtonName = "View"; ///< Name of the parent button.
    [SerializeField] private string subButtonName = "ActionLog"; ///< Name of this sub-button.
    [SerializeField] private Transform raycastOrigin; ///< Transform of the right VR controller.
    [SerializeField] private LayerMask interactionLayer; ///< Layer mask to filter raycast hits.
    [SerializeField] private float raycastDistance = 10f; ///< Maximum raycast distance.

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        // Verify raycast origin
        if (raycastOrigin == null)
        {
            Debug.LogError($"[ActionLogVRRaycast] Raycast origin not set on {gameObject.name}. Please assign the right VR controller Transform.");
            enabled = false;
            return;
        }

        // Verify FileManager instance
        if (FileManager.Instance == null)
        {
            Debug.LogError($"[ActionLogVRRaycast] FileManager instance is null. Ensure FileManager is in the scene.");
            enabled = false;
            return;
        }

        // Verify the GameObject has a Collider
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError($"[ActionLogVRRaycast] No Collider found on {gameObject.name}. Please attach a Collider component.");
            enabled = false;
            return;
        }

        Debug.Log($"[ActionLogVRRaycast] Initialized for {subButtonName} with raycast origin {raycastOrigin.name}");
    }

    /// <summary>
    /// Called every frame to check for VR raycast-based interactions.
    /// </summary>
    void Update()
    {
        // Perform raycast from right VR controller
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, interactionLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                OnInteract(mainButtonName, subButtonName);
            }
        }
    }

    /// <summary>
    /// Called when the ActionLog sub-button is interacted with via VR raycast.
    /// </summary>
    
    void OnInteract(string objectName, string subButtonName)
    {
        // Log the interaction to the console
        Debug.Log($"[ActionLogVRRaycast] VR Interaction: {objectName}, SubButton: {subButtonName}");

        // Record the interaction with timestamp in the CSV
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        try
        {
            //FileManager.Instance.WriteToFile(objectName, timestamp, subButtonName);
            Debug.Log($"[ActionLogVRRaycast] Recorded to CSV: Index={FileManager.Instance.GetClickedObjectsCount()}, Timestamp={timestamp}, ObjectName={objectName}, SubButtonName={subButtonName}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ActionLogVRRaycast] Error writing to CSV: {ex.Message}");
        }

        // Open the CSV file
        try
        {
            FileManager.Instance.OpenFile();
            Debug.Log($"[ActionLogVRRaycast] Opened CSV file for {objectName}, SubButton: {subButtonName}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ActionLogVRRaycast] Error opening CSV file: {ex.Message}");
        }
    }
}