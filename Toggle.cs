using UnityEngine;
using UnityEngine.UI;

public class RayBasedToggleSelector : MonoBehaviour
{
    public Transform rightController; // Assign your right controller transform
    public float rayLength = 10f;
    public LineRenderer lineRenderer; // Optional, to visualize the ray

    private Toggle lastToggle = null;

    void Update()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;

        // Visualize the ray
        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength);
        }

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject hitObject = hit.collider.gameObject;
            Toggle toggle = hitObject.GetComponent<Toggle>();

            if (toggle != null && toggle != lastToggle)
            {
                toggle.isOn = true; // Auto-select on hover
                lastToggle = toggle;
            }
        }
        else
        {
            lastToggle = null;
        }
    }
}
