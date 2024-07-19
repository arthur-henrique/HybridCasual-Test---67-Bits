using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick joystick;
    [SerializeField]
    private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Animator animator;

    private Finger movementFinger;
    private Vector2 movementInput;
    private float movementSpeed = 10f;
    private float currentSpeed = 0f;
    

    [SerializeField]
    private Transform stackParent; // Reference to the StackParent

    private float gravity = -9.81f; // Gravity value
    private float verticalVelocity = 0f; // Vertical velocity
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.onFingerDown += HandleFingerDown;
        ETouch.onFingerUp += HandleLoseFinger;
        ETouch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.onFingerDown -= HandleFingerDown;
        ETouch.onFingerUp -= HandleLoseFinger;
        ETouch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

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

    private void Update()
    {
        // Create a vector for the movement input
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);

        if (move.magnitude > 0.1f) // Adjust the threshold as needed
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
