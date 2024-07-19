using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeClothesButton : MonoBehaviour
{
    // Indicates whether the button is locked and requires payment to unlock
    public bool isLocked = true;

    // Materials for the shirt and pants
    public Material shirtMaterial;
    public Material pantsMaterial;

    // UI Elements for displaying the price
    public GameObject priceGroup; // Group that contains price-related UI elements
    public TMP_Text priceText;    // Text element to display the price

    // Cost to unlock the button
    public int cost = 1;

    // Called when the script instance is being loaded
    private void Start()
    {
        // Check if the button is locked
        if (isLocked)
        {
            // Display the price UI elements and set the price text
            priceGroup.SetActive(true);
            priceText.text = string.Format("Cost: {0}", cost);
        }
        else
        {
            // Hide the price UI elements if the button is not locked
            priceGroup.SetActive(false);
        }
    }

    // Called when the button is pressed
    public void OnButtonPress()
    {
        // Check if the button is locked
        if (isLocked)
        {
            // Check if the player has enough currency to unlock the button
            if (GameManager.instance.playerCurrency >= cost)
            {
                // Unlock the button
                isLocked = false;

                // Hide the price UI elements
                priceGroup.SetActive(false);

                // Deduct the cost from the player's currency
                GameManager.instance.playerCurrency -= cost;

                // Update the UI to reflect the new coin amount
                GameManager.instance.UpdateCoinText();

                // Update the player's outfit with the new materials
                GameManager.instance.UpdatePlayerOutfit(shirtMaterial, pantsMaterial);
            }
        }
        else
        {
            // If the button is not locked, just update the player's outfit
            GameManager.instance.UpdatePlayerOutfit(shirtMaterial, pantsMaterial);
        }
    }
}
