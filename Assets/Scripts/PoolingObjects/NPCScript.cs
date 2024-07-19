using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class NPCScript : MonoBehaviour, IPooledObject
{
    public float speed = 3f; // Movement speed of the NPC
    public float lifeTime = 10f; // Lifetime of the NPC before it gets disabled
    public bool canMove = true; // Flag to check if the NPC can move

    public Animator animator; // Reference to the Animator component
    public RagdollControl ragdollControl; // Reference to the RagdollControl component

    // Method called when the object is spawned from the pool
    public void OnObjectSpawn()
    {
        StartCoroutine(DisableAfterTime()); // Start coroutine to disable the NPC after its lifetime
        animator.SetFloat("Speed", 1); // Set the animation speed to 1
        canMove = true; // Enable movement
    }

    // Method called when the NPC gets punched
    public void OnGotPunched()
    {
        animator.SetFloat("Speed", 0); // Stop the animation
        canMove = false; // Disable movement
        StartCoroutine(DisableAfterPunch()); // Start coroutine to disable the NPC after getting punched
    }

    // Coroutine to disable the NPC after its lifetime
    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(lifeTime); // Wait for the lifetime duration
        gameObject.SetActive(false); // Deactivate the NPC
        ragdollControl.ResetTarget(); // Reset the ragdoll to its initial state
    }

    // Coroutine to disable the NPC after getting punched
    IEnumerator DisableAfterPunch()
    {
        StopCoroutine(DisableAfterTime()); // Stop the lifetime coroutine
        yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
        gameObject.SetActive(false); // Deactivate the NPC
        ragdollControl.ResetTarget(); // Reset the ragdoll to its initial state
    }

    // Update method called once per frame
    void Update()
    {
        if (canMove) // Check if the NPC can move
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move the NPC forward
        }
    }
}
