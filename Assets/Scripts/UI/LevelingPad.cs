using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingPad : MonoBehaviour
{
    public float activationTimer = 2f; // Time required for the pad to activate
    [SerializeField] private Image timerImage; // UI image to show the activation progress
    private Coroutine activationCoroutine; // Coroutine for the activation process
    public GameObject OptionCanvas; // Canvas to display options when pad is activated
    public TMP_Text costText; // Text to display the cost of the upgrade
    private int currentCost = 10; // Initial cost of the upgrade

    // Start is called before the first frame update
    void Start()
    {
        // Update the cost text with the initial cost
        UpdateCostText(currentCost);
    }

    // Triggered when a collider enters the pad's trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop any existing activation coroutine
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
            }
            // Start a new activation coroutine
            activationCoroutine = StartCoroutine(ActivatePad());
        }
    }

    // Triggered when a collider exits the pad's trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the activation coroutine if it's running
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
                activationCoroutine = null;
            }
            // Reset the timer image and hide the option canvas
            timerImage.fillAmount = 0;
            OptionCanvas.SetActive(false);
        }
    }

    // Coroutine to handle the activation process
    IEnumerator ActivatePad()
    {
        float elapsedTime = 0f;
        while (elapsedTime < activationTimer)
        {
            // Update the timer image fill amount based on elapsed time
            timerImage.fillAmount = elapsedTime / activationTimer;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the timer image is fully filled and show the option canvas
        timerImage.fillAmount = 1f;
        OptionCanvas.SetActive(true);
    }

    // Update the cost text with the given cost value
    public void UpdateCostText(int cost)
    {
        costText.text = string.Format("Cost: {0}", cost);
    }

    // Method to handle the purchase of the stack upgrade
    public void BuyStackUp()
    {
        // Check if the player has enough currency to buy the upgrade
        if (GameManager.instance.playerCurrency >= currentCost)
        {
            // Deduct the cost from the player's currency
            GameManager.instance.playerCurrency -= currentCost;
            // Update the UI with the new currency amount
            GameManager.instance.UpdateCoinText();
            // Increase the cost for the next upgrade
            currentCost += 10;
            // Increase the player's stack size
            GameManager.instance.IncreaseStack();
            // Update the cost text with the new cost
            UpdateCostText(currentCost);
        }
    }
}
