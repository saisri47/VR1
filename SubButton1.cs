using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles sub-button visibility for VR UI menus.
/// When the VR controller ray points at this main button,
/// its associated sub-buttons are shown and others are hidden.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapathi@gmail.com
/// </remarks>
public class SubButton1 : MonoBehaviour
{
    public Transform rayOrigin; // The VR controller's ray origin (e.g., controller tip)
    public GameObject[] subButtons; // Array of individual sub-button GameObjects
    public float rayDistance = 10f; // Maximum raycast distance
    private bool isPointedAt = false; // Tracks if ray is pointing at this button
    private Collider buttonCollider; // Reference to the button's collider
    private static List<SubButton1> allButtons = new List<SubButton1>(); // Tracks all main buttons

    /// <summary>
    /// Called on start. Initializes collider, registers button, and hides sub-buttons.
    /// </summary>
    void Start()
    {
        // Get the button's collider
        buttonCollider = GetComponent<Collider>();
        if (buttonCollider == null)
        {
            Debug.LogError($"No Collider found on {gameObject.name}. Please add a Collider component.");
            return;
        }

        // Add this button to the static list
        allButtons.Add(this);

        // Ensure all sub-buttons are hidden at start
        if (subButtons != null && subButtons.Length > 0)
        {
            foreach (GameObject subButton in subButtons)
            {
                if (subButton != null)
                {
                    subButton.SetActive(false);
                }
                else
                {
                    Debug.LogWarning($"Sub-button reference is null in {gameObject.name}. Check the subButtons array in the Inspector.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No sub-buttons assigned to {gameObject.name}. Check the subButtons array in the Inspector.");
        }

        // Check rayOrigin
        if (rayOrigin == null)
        {
            Debug.LogError($"rayOrigin is not assigned on {gameObject.name}. Please assign the VR controller's Transform.");
        }
    }

    /// <summary>
    /// Called when the GameObject is destroyed. Unregisters the button.
    /// </summary>
    void OnDestroy()
    {
        // Remove this button from the static list
        allButtons.Remove(this);
    }

    /// <summary>
    /// Updates raycasting every frame to show/hide sub-buttons based on pointing direction.
    /// </summary>
    void Update()
    {
        if (rayOrigin == null || buttonCollider == null) return;

        // Perform raycast from the VR controller's ray origin
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Check if the ray hits this button's collider
            if (hit.collider == buttonCollider && !isPointedAt)
            {
                // Hide sub-buttons of all other main buttons
                foreach (SubButton1 otherButton in allButtons)
                {
                    if (otherButton != this && otherButton.isPointedAt)
                    {
                        otherButton.HideSubButtons();
                        otherButton.isPointedAt = false;
                        //Debug.Log($"Hiding sub-buttons of {otherButton.gameObject.name}");
                    }
                }

                isPointedAt = true;
                ShowSubButtons();
                //Debug.Log($"Showing sub-buttons of {gameObject.name}");
            }
        }
        else if (isPointedAt)
        {
            // If ray is no longer hitting this button, hide sub-buttons
            isPointedAt = false;
            HideSubButtons();
            //Debug.Log($"Hiding sub-buttons of {gameObject.name} (ray no longer pointing)");
        }
    }

    /// <summary>
    /// Shows all sub-buttons associated with this main button.
    /// </summary>
    
    private void ShowSubButtons()
    {
        if (subButtons == null) return;

        // Activate all sub-buttons
        foreach (GameObject subButton in subButtons)
        {
            if (subButton != null)
            {
                subButton.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Hides all sub-buttons associated with this main button.
    /// </summary>
    private void HideSubButtons()
    {
        if (subButtons == null) return;

        // Deactivate all sub-buttons
        foreach (GameObject subButton in subButtons)
        {
            if (subButton != null)
            {
                subButton.SetActive(false);
            }
        }
    }
}