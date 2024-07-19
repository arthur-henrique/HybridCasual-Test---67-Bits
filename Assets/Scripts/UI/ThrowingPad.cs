using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThrowingPad : MonoBehaviour
{
    public float activationTimer = 2f;
    [SerializeField] private Image timerImage;
    private Coroutine activationCoroutine;
    public TargetStacker stackHandler; // Reference to your stack handler script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
            }
            activationCoroutine = StartCoroutine(ActivatePad());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
                activationCoroutine = null;
            }
            timerImage.fillAmount = 0;
        }
    }

    IEnumerator ActivatePad()
    {
        float elapsedTime = 0f;
        while (elapsedTime < activationTimer)
        {
            timerImage.fillAmount = elapsedTime / activationTimer;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timerImage.fillAmount = 1f;
        // Empty the stack once the timer is completed
        stackHandler.EmptyStack();
    }
}
