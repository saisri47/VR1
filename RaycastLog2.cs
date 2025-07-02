using System;
using UnityEngine;

/// <summary>
/// Performs a raycast from the object’s forward direction to detect UI Buttons.
/// Logs the main button and sub-button names (if nested),
/// and writes the data to a file with a timestamp using FileManager.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapathi@gmail.com
/// </remarks>
public class RaycastLog2 : MonoBehaviour
{
    LayerMask layerMask;
    GameObject lastHitButton;

    /// <summary>
    /// Called at the beginning. Initializes the UI layer mask.
    /// </summary>
    
    void Start()
    {
        layerMask = LayerMask.GetMask("UI");
    }

    /// <summary>
    /// Called on a fixed time interval. Performs a raycast to detect UI elements in front of the GameObject.
    /// </summary>
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<UnityEngine.UI.Button>() != null)
            {
                if (hitObject != lastHitButton)
                {
                    lastHitButton = hitObject;

                    // Get the current UTC timestamp in milliseconds since Unix epoch
                    long timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);

                    // Check if the button is a child of a parent button
                    Transform parent = hitObject.transform.parent;
                    string objectName = hitObject.name;
                    string subButtonName = "";

                    while (parent != null)
                    {
                        if (parent.GetComponent<UnityEngine.UI.Button>() != null)
                        {
                            // Found a parent button
                            objectName = parent.name;
                            subButtonName = hitObject.name;
                            break;
                        }
                        parent = parent.parent;
                    }

                    // Log button info and write to file
                    Debug.Log($"Main: {objectName}, Sub: {subButtonName}");
                    FileManager.Instance.WriteToFile(objectName, timestamp, subButtonName);
                }
            }
            else
            {
                // Log non-button hit if different from last hit
                if (hitObject != lastHitButton)
                {
                    Debug.Log("Hit object: " + hitObject.name + " (not a button)");
                    lastHitButton = hitObject;
                }
            }
        }
        else
        {
            // No hit — clear lastHitButton to allow logging on next detection
            if (lastHitButton != null)
            {
                lastHitButton = null;
            }
        }
    }
}
