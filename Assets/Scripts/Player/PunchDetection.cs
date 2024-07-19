using System.Collections;
using UnityEngine;

public class PunchDetection : MonoBehaviour
{
    public float punchForce = 35f; // The force applied to the ragdoll when punched
    public Animator animator; // The animator component for triggering punch animations
    public Transform player; // The player transform to rotate towards targets
    public float rotationSpeed = 10f; // Speed of rotation towards the target
    public float punchCooldown = 1f; // Cooldown time between punches

    private bool isPunching = false; // Flag to prevent multiple punches
    private RagdollControl currentRagdoll; // Reference to the current target's ragdoll
    private NPCScript npcScript; // Reference to the NPC script on the target
    private Vector3 punchDirection; // Direction of the punch
    private TargetStacker targetStacker; // Reference to the TargetStacker component

    // Initialize the target stacker reference
    private void Start()
    {
        targetStacker = player.GetComponent<TargetStacker>();
    }

    // Handle collision with targets
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !isPunching)
        {
            StartCoroutine(Punch(other));
        }
    }

    // Handle exiting collision with targets
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target") && !isPunching)
        {
            currentRagdoll = null;
            animator.ResetTrigger("Punch");
            StopAllCoroutines();
        }
    }

    // Coroutine to handle the punch action
    private IEnumerator Punch(Collider target)
    {
        // Trigger the ragdoll effect
        currentRagdoll = target.GetComponent<RagdollControl>();
        if (currentRagdoll != null)
        {
            npcScript = target.GetComponent<NPCScript>();
            // Rotate the player towards the target
            Vector3 directionToTarget = target.transform.position - player.position;
            directionToTarget.y = 0; // Keep the rotation in the horizontal plane
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Store the punch direction
            punchDirection = directionToTarget.normalized;

            // Trigger the punch animation immediately
            animator.SetTrigger("Punch");

            // Rotate the player while the punch animation plays
            StartCoroutine(RotateTowardsTarget(targetRotation));
        }

        yield return new WaitForSeconds(punchCooldown);
        isPunching = false;
    }

    // Coroutine to smoothly rotate the player towards the target
    private IEnumerator RotateTowardsTarget(Quaternion targetRotation)
    {
        while (Quaternion.Angle(player.rotation, targetRotation) > 0.1f)
        {
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        player.rotation = targetRotation; // Ensure exact rotation
    }

    // Method to be called by the animation event to apply the punch force
    public void ApplyPunchForce()
    {
        if (currentRagdoll != null)
        {
            currentRagdoll.EnableRagdoll(punchDirection * punchForce);
            npcScript.OnGotPunched();
            // Add the target to the stack
            targetStacker.AddTargetToStack();
            currentRagdoll = null;
        }
    }
}
