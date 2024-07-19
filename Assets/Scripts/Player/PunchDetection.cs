using System.Collections;
using UnityEngine;

public class PunchDetection : MonoBehaviour
{
    public float punchForce = 35f;
    public Animator animator; // Reference to the player's Animator
    public Transform player; // Reference to the player's Transform
    public float rotationSpeed = 10f; // Increase speed of rotation towards the target
    public float punchCooldown = 1f; // Cooldown time between punches

    private bool isPunching = false; // Flag to prevent multiple punches
    private RagdollControl currentRagdoll; // Reference to the current target's ragdoll
    private NPCScript npcScript;
    private Vector3 punchDirection; // Direction of the punch
    private TargetStacker targetStacker; // Reference to the TargetStacker component

    private void Start()
    {
        // Get the TargetStacker component from the player
        targetStacker = player.GetComponent<TargetStacker>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !isPunching)
        {
            StartCoroutine(Punch(other));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target") && !isPunching)
        {
            currentRagdoll = null;
            animator.ResetTrigger("Punch");
            StopAllCoroutines();
        }
    }

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

        // Wait for the punch cooldown
        yield return new WaitForSeconds(punchCooldown);
        isPunching = false;
    }

    private IEnumerator RotateTowardsTarget(Quaternion targetRotation)
    {
        while (Quaternion.Angle(player.rotation, targetRotation) > 0.1f)
        {
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        player.rotation = targetRotation; // Ensure exact rotation
    }

    // Method to be called by the animation event
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
        // Add the target to the stack
        
    }
}
