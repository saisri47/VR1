using UnityEngine;
using UnityEngine.XR; // Use XR namespace for InputDevice and CommonUsages
/// <summary>
/// Logs button presses and counts them for left and right VR controllers.
/// </summary>
/// <remarks>
/// Code made by Saisri, email: saisribogapathi@gmail.com
/// </remarks>
public class ButtonLogger2 : MonoBehaviour
{
    // Input devices for left and right controllers
    private UnityEngine.XR.InputDevice leftController;
    private UnityEngine.XR.InputDevice rightController;

    // Track previous button states for left and right controllers
    private bool leftPrimary, leftSecondary, leftTrigger, leftGrip;
    private bool rightPrimary, rightSecondary, rightTrigger, rightGrip;

    // Counters for button presses
    private int leftPrimaryCount, leftSecondaryCount, leftTriggerCount, leftGripCount;
    private int rightPrimaryCount, rightSecondaryCount, rightTriggerCount, rightGripCount;

    /// <summary>
    /// Initializes input devices and counters.
    /// </summary>
    
    void Start()
    {
        // Get the left and right controller devices
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Initialize counters to zero
        leftPrimaryCount = leftSecondaryCount = leftTriggerCount = leftGripCount = 0;
        rightPrimaryCount = rightSecondaryCount = rightTriggerCount = rightGripCount = 0;
    }

    /// <summary>
    /// Checks input states every frame and logs button presses/releases.
    /// </summary>
    
    void Update()
    {
        // Check for button presses and releases on both controllers
        CheckControllerButtons(leftController, "Left Controller", ref leftPrimary, ref leftSecondary, ref leftTrigger, ref leftGrip,
            ref leftPrimaryCount, ref leftSecondaryCount, ref leftTriggerCount, ref leftGripCount);
        CheckControllerButtons(rightController, "Right Controller", ref rightPrimary, ref rightSecondary, ref rightTrigger, ref rightGrip,
            ref rightPrimaryCount, ref rightSecondaryCount, ref rightTriggerCount, ref rightGripCount);
    }

    /// <summary>
    /// Checks the button states of a controller and logs transitions with counts.
    /// </summary>
    
    private void CheckControllerButtons(UnityEngine.XR.InputDevice controller, string controllerName,
        ref bool prevPrimary, ref bool prevSecondary, ref bool prevTrigger, ref bool prevGrip,
        ref int primaryCount, ref int secondaryCount, ref int triggerCount, ref int gripCount)
    {
        if (controller.isValid)
        {
            // Check primary button
            if (controller.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton))
            {
                if (primaryButton != prevPrimary)
                {
                    if (primaryButton) // Button pressed
                    {
                        primaryCount++;
                        Debug.Log($"{controllerName}: Primary Button Pressed (Count: {primaryCount})");
                    }
                    else // Button released
                    {
                        Debug.Log($"{controllerName}: Primary Button Released");
                    }
                    prevPrimary = primaryButton;
                }
            }

            // Check secondary button
            if (controller.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton))
            {
                if (secondaryButton != prevSecondary)
                {
                    if (secondaryButton) // Button pressed
                    {
                        secondaryCount++;
                        Debug.Log($"{controllerName}: Secondary Button Pressed (Count: {secondaryCount})");
                    }
                    else // Button released
                    {
                        Debug.Log($"{controllerName}: Secondary Button Released");
                    }
                    prevSecondary = secondaryButton;
                }
            }

            // Check trigger button
            if (controller.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton))
            {
                if (triggerButton != prevTrigger)
                {
                    if (triggerButton) // Button pressed
                    {
                        triggerCount++;
                        Debug.Log($"{controllerName}: Trigger Button Pressed (Count: {triggerCount})");
                    }
                    else // Button released
                    {
                        Debug.Log($"{controllerName}: Trigger Button Released");
                    }
                    prevTrigger = triggerButton;
                }
            }

            // Check grip button
            if (controller.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton))
            {
                if (gripButton != prevGrip)
                {
                    if (gripButton) // Button pressed
                    {
                        gripCount++;
                        Debug.Log($"{controllerName}: Grip Button Pressed (Count: {gripCount})");
                    }
                    else // Button released
                    {
                        Debug.Log($"{controllerName}: Grip Button Released");
                    }
                    prevGrip = gripButton;
                }
            }
        }
    }
}