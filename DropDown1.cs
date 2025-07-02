using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Handles VR ray-based interaction with a dropdown-style button menu in Unity UI.
/// When the main button is pointed at, it reveals sub-buttons.
/// If the ray stops pointing, the dropdown closes.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapathi@gmail.com
/// </remarks>

public class VRRaycastDropdown1 : MonoBehaviour
{
    public Transform raycastSource; 
    public float raycastDistance = 10f; 
    public List<GameObject> subButtons; 
    public static VRRaycastDropdown1 activeDropdown; 
    private bool isDropdownActive = false; 
    private int interactionCount = 0; 

    /// <summary>
    /// Called at the start of the script instance.
    /// </summary>
    void Start()
    {
        // Ensure raycastSource is assigned
        if (raycastSource == null)
        {
            Debug.LogWarning($"Raycast Source not assigned in {gameObject.name}. Please assign a VR controller or headset transform.");
        }

        // Ensure the main button has an Image component (for compatibility with UI)
        if (!GetComponent<Image>())
        {
            Debug.LogWarning($"No Image component found on {gameObject.name}. Ensure this is a UI button.");
        }

        // Ensure sub-buttons are initially inactive and have necessary components
        if (subButtons == null || subButtons.Count == 0)
        {
            Debug.LogWarning($"No sub-buttons assigned to {gameObject.name}. Check the subButtons list in the Inspector.");
        }
        else
        {
            foreach (GameObject subButton in subButtons)
            {
                if (subButton != null)
                {
                    subButton.SetActive(false);
                    if (!subButton.GetComponent<Collider>())
                    {
                        subButton.AddComponent<BoxCollider>();
                    }
                    if (!subButton.GetComponent<Image>())
                    {
                        Debug.LogWarning($"Sub-button {subButton.name} is missing an Image component.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Null sub-button reference in {gameObject.name}. Check the subButtons list in the Inspector.");
                }
            }
        }

        // Ensure the main button has a collider for raycast detection
        if (!GetComponent<Collider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    /// <summary>
    /// Called every frame to handle raycast-based interaction.
    /// </summary>
    void Update()
    {
        if (raycastSource == null)
            return;

        // Perform raycast from the source
        Ray ray = new Ray(raycastSource.position, raycastSource.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the hit object is this dropdown's main button
            if (hit.collider.gameObject == gameObject && !isDropdownActive)
            {
                // Close the active dropdown if another dropdown is pointed at
                if (activeDropdown != null && activeDropdown != this)
                {
                    activeDropdown.CloseDropdown();
                }

                isDropdownActive = true;
                interactionCount++;

                // Show sub-buttons
                if (subButtons != null)
                {
                    foreach (GameObject subButton in subButtons)
                    {
                        if (subButton != null)
                        {
                            subButton.SetActive(true);
                        }
                    }
                }

                // Set this dropdown as the active one
                activeDropdown = this;

                Debug.Log($"#{interactionCount} button pointed at: {GetMainButtonName()}");
            }
        }
        else if (isDropdownActive && activeDropdown == this)
        {
            // Close the dropdown if the ray is no longer pointing at the main button
            CloseDropdown();
            interactionCount++;
            Debug.Log($"#{interactionCount} button no longer pointed at: {GetMainButtonName()}");
        }
    }

    /// <summary>
    /// Closes the dropdown menu.
    /// </summary>
    public void CloseDropdown()
    {
        // Hide sub-buttons
        if (subButtons != null)
        {
            foreach (GameObject subButton in subButtons)
            {
                if (subButton != null)
                {
                    subButton.SetActive(false);
                }
            }
        }

        // If this dropdown is the active one, set activeDropdown to null
        if (activeDropdown == this)
        {
            activeDropdown = null;
        }

        isDropdownActive = false;
    }

    /// <summary>
    /// Retrieves the name of the main button.
    /// </summary>
    /// <returns>The name of the main button.</returns>
    private string GetMainButtonName()
    {
        return gameObject.name;
    }
}