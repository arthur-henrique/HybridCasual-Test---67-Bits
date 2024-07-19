using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThrowingPad : MonoBehaviour
{
    // Duration for which the throwing pad is active
    public float activationTimer = 2f;

    // Reference to the UI Image component that represents the timer
    [SerializeField] private Image timerImage;

    // Coroutine that handles the activation of the throwing pad
    private Coroutine activationCoroutine;

    // Reference to the TargetStacker that manages the stack
    public TargetStacker stackHandler;

    // Called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // If there is an active coroutine, stop it to reset the timer
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
            }

            // Start a new coroutine to activate the throwing pad
            activationCoroutine = StartCoroutine(ActivatePad());
        }
    }

    // Called when another collider exits the trigger collider attached to this GameObject
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider that exited the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // If there is an active coroutine, stop it and set it to null
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
                activationCoroutine = null;
            }

            // Reset the timer image fill amount to 0
            timerImage.fillAmount = 0;
        }
    }

    // Coroutine to handle the activation timer and actions
    IEnumerator ActivatePad()
    {
        float elapsedTime = 0f;

        // Loop until the elapsed time is less than the activation timer
        while (elapsedTime < activationTimer)
        {
            // Update the fill amount of the timer image to reflect the elapsed time
            timerImage.fillAmount = elapsedTime / activationTimer;

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the fill amount is set to 1 (full) when the timer completes
        timerImage.fillAmount = 1f;

        // Call method to empty the stack once the timer is completed
        stackHandler.EmptyStack();
    }
}
