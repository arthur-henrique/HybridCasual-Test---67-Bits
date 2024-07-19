using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick joystick; // The joystick used for player movement
    [SerializeField]
    private Vector2 joystickSize = new Vector2(300, 300); // The size of the joystick
    [SerializeField]
    private CharacterController controller; // The character controller component
    [SerializeField]
    private Animator animator; // The animator component

    private Finger movementFinger; // The finger currently used for movement
    private Vector2 movementInput; // The input vector for movement
    private float movementSpeed = 10f; // The speed at which the player moves

    [SerializeField]
    private Transform stackParent; // The parent object for the stack of objects

    private float gravity = -9.81f; // Gravity value
    private float verticalVelocity = 0f; // Vertical velocity for gravity handling

    // Enable enhanced touch support and subscribe to touch events
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.onFingerDown += HandleFingerDown;
        ETouch.onFingerUp += HandleLoseFinger;
        ETouch.onFingerMove += HandleFingerMove;
    }

    // Disable enhanced touch support and unsubscribe from touch events
    private void OnDisable()
    {
        ETouch.onFingerDown -= HandleFingerDown;
        ETouch.onFingerUp -= HandleLoseFinger;
        ETouch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    // Handle finger movement for joystick control
    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2f;
            ETouch currentTouch = movementFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
            }
            joystick.Knob.anchoredPosition = knobPosition;
            movementInput = knobPosition / maxMovement;
        }
    }

    // Handle the loss of a finger from the screen
    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movementInput = Vector2.zero;
        }
    }

    // Handle the initial touch on the screen
    private void HandleFingerDown(Finger finger)
    {
        if (movementFinger == null)
        {
            movementFinger = finger;
            movementInput = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.RectTransform.sizeDelta = joystickSize;
            joystick.RectTransform.anchoredPosition = ClampStartPosition(finger.screenPosition);
        }
    }

    // Clamp the joystick start position to prevent it from going off-screen
    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
        {
            startPosition.x = joystickSize.x / 2;
        }
        if (startPosition.y < joystickSize.y / 2)
        {
            startPosition.y = joystickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - joystickSize.y / 2)
        {
            startPosition.y = Screen.height - joystickSize.y / 2;
        }
        return startPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        // Create a movement vector from the joystick input
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);

        if (move.magnitude > 0.1f) // Check if there is significant movement input
        {
            // Ensure move direction is normalized
            move.Normalize();

            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);

            // Apply the rotation immediately
            transform.rotation = targetRotation;

            // Tilt the StackParent based on player's forward direction
            Vector3 tiltDirection = -transform.forward; // Always tilt backward relative to the player
            Quaternion tiltRotation = Quaternion.Euler(move.magnitude * -10f, 0, tiltDirection.x * 2f); // Adjust the tilt factors as needed
            stackParent.localRotation = Quaternion.Slerp(stackParent.localRotation, tiltRotation, Time.deltaTime * 2f); // Adjust the tilt speed as needed
        }
        else
        {
            // Smoothly return StackParent to its original rotation
            stackParent.localRotation = Quaternion.Slerp(stackParent.localRotation, Quaternion.identity, Time.deltaTime * 2f);
        }

        // Handle gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // Small value to ensure the character stays grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply movement and gravity
        Vector3 velocity = move * movementSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // Update animator parameters
        animator.SetFloat("Speed", move.magnitude);
    }
}
