using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

/// <summary>
/// Controls toggle selection and button interaction in VR using a raycast from the right controller.
/// This script uses a raycast from the right controller to interact with UI toggles and a submit button.
/// When the ray hits a toggle, it selects it. If the submit button is hit while the VR Controller toggle is selected
/// and the trigger is pressed, the input mode panel is deactivated.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapathi@gmail.com
/// </remarks>
public class RayBasedToggleSelector1 : MonoBehaviour
{
    public Transform rightController; // Assign your right controller transform
    public float rayLength = 10f;
    public LineRenderer lineRenderer; // Optional, to visualize the ray

    public Toggle vrControllerToggle;           // Drag VR Controller toggle here
    public GameObject modeOfInputPanel;         // Drag Mode of Input panel here
    public Button submitButton;                 // Drag Submit button here

    private Toggle lastToggle = null;

    /// <summary>
    /// Updates the raycast and handles toggle selection and button interaction.
    /// </summary>
    void Update()
    {
        // Create a ray from the right controller's position and forward direction.
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;

        // Visualize the ray
        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength);
        }

        // Perform raycast and check for hits.
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check for toggle
            Toggle toggle = hitObject.GetComponent<Toggle>();
            if (toggle != null && toggle != lastToggle)
            {
                toggle.isOn = true;
                lastToggle = toggle;
            }

            // Check if ray is over Submit and VR Controller is selected
            if (hitObject == submitButton.gameObject && vrControllerToggle.isOn && UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed)
)
            {
                modeOfInputPanel.SetActive(false);
            }
        }
        else
        {
            lastToggle = null;
        }
    }
}
