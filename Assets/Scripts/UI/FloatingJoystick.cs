using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensure that the GameObject this script is attached to has a RectTransform component
[RequireComponent(typeof(RectTransform))]
// Prevent multiple instances of this component from being added to the same GameObject
[DisallowMultipleComponent]
public class FloatingJoystick : MonoBehaviour
{
    // Public reference to the RectTransform component of the joystick
    public RectTransform RectTransform;

    // Public reference to the RectTransform component of the joystick knob
    public RectTransform Knob;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Initialize the RectTransform variable with the RectTransform component attached to the same GameObject
        RectTransform = GetComponent<RectTransform>();
    }
}
