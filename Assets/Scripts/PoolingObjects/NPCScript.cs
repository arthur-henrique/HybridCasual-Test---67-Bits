using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class NPCScript : MonoBehaviour, IPooledObject
{
    public float speed = 3f;
    public float lifeTime = 10f;
    public bool canMove = true;

    public Animator animator;
    public RagdollControl ragdollControl;
    public void OnObjectSpawn()
    {
        StartCoroutine(DisableAfterTime());
        animator.SetFloat("Speed", 1);
        canMove = true;
    }
    public void OnGotPunched()
    {
        animator.SetFloat("Speed", 0);
        canMove = false;
        StartCoroutine(DisableAfterPunch());
    }
    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
        ragdollControl.ResetTarget();
    }
    IEnumerator DisableAfterPunch()
    {
        StopCoroutine(DisableAfterTime());
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        ragdollControl.ResetTarget();

    }

    void Update()
    {
        if (canMove)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        
    }
}

