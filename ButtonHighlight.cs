using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Highlights a UI Button when pointed at with an XR Ray Interactor (e.g., VR controller).
/// Restores the original button color when not pointed at.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapathi@gmail.com
/// </remarks>
public class ButtonHighlight : MonoBehaviour
{
    [SerializeField] private XRRayInteractor rayInteractor; // Reference to the VR controller's ray interactor
    [SerializeField] private float rayMaxDistance = 10f; // Maximum raycast distance
    [SerializeField] private Color highlightColor = Color.gray; // Button highlight color

    private Button lastHitButton; // Tracks the last button hit by the ray
    private Color originalColor; // Stores the button's original color

    /// <summary>
    /// Called once per frame to check raycast hits and apply/remove highlights accordingly.
    /// </summary>
    void Update()
    {
        // Try to get the current raycast hit from the ray interactor
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.distance <= rayMaxDistance)
        {
            // Check if the ray hits a UI button
            Button button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                // If hitting a new button, reset the previous button's color
                if (lastHitButton != null && lastHitButton != button)
                {
                    ResetButtonColor(lastHitButton);
                }

                // Highlight the current button
                HighlightButton(button);
                lastHitButton = button;
            }
            else
            {
                // If not hitting a button, reset the last button's color
                if (lastHitButton != null)
                {
                    ResetButtonColor(lastHitButton);
                    lastHitButton = null;
                }
            }
        }
        else
        {
            // If ray doesn't hit anything, reset the last button's color
            if (lastHitButton != null)
            {
                ResetButtonColor(lastHitButton);
                lastHitButton = null;
            }
        }
    }

    /// <summary>
    /// Highlights the specified button by changing its color.
    /// </summary>
    
    private void HighlightButton(Button button)
    {
        // Store the original color if not already stored
        if (lastHitButton != button)
        {
            var colors = button.colors;
            originalColor = colors.normalColor;
        }

        // Set the highlight color
        var newColors = button.colors;
        newColors.normalColor = highlightColor;
        newColors.selectedColor = highlightColor;
        button.colors = newColors;
    }

    /// <summary>
    /// Resets the specified button's color to its original color.
    /// </summary>
    
    private void ResetButtonColor(Button button)
    {
        // Restore the original color
        var colors = button.colors;
        colors.normalColor = originalColor;
        colors.selectedColor = originalColor;
        button.colors = colors;
    }
}