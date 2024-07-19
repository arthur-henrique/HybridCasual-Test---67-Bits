using UnityEngine;

public class RagdollControl : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private Rigidbody[] rigidbodies; // Array to hold all child Rigidbodies
    private Collider[] colliders; // Array to hold all child Colliders
    [SerializeField] private Collider mainColl; // Main collider for the character

    private Vector3 initialPosition; // Initial position of the character
    private Quaternion initialRotation; // Initial rotation of the character

    void Start()
    {
        // Get references to Animator, Rigidbodies, and Colliders
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        // Store initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Disable Ragdoll initially
        SetRagdollState(false);
    }

    public void EnableRagdoll(Vector3 force)
    {
        // Disable the Animator
        animator.enabled = false;

        // Enable Ragdoll
        SetRagdollState(true);

        // Apply force to the Ragdoll
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void SetRagdollState(bool state)
    {
        // Set all child Rigidbodies to kinematic or non-kinematic
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !state;
        }

        // Enable or disable all child Colliders
        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }

        // Set the main collider to the opposite state of the ragdoll
        mainColl.enabled = !state;
    }

    public void ResetTarget()
    {
        // Re-enable the Animator
        animator.enabled = true;

        // Disable Ragdoll
        SetRagdollState(false);

        // Reset the Rigidbody velocities
        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false; // Temporarily disable kinematic to reset velocity
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // Re-enable kinematic
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // Ensure the main collider is enabled
        mainColl.enabled = true;

        // Reset the transform to its original state if needed
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
