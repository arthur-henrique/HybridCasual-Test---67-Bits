using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance of the GameManager

    public int playerCurrency; // Player's current amount of currency
    public int playerStackSize = 3; // Initial size of the player's stack

    public TMP_Text currencyText; // UI Text component to display the player's currency

    public SkinnedMeshRenderer playerShirt; // Renderer for the player's shirt
    public SkinnedMeshRenderer playerPants; // Renderer for the player's pants

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Implement the singleton pattern
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // Destroy duplicate GameManager instances
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Update the currency text on start
        UpdateCoinText();
    }

    // Method to increase the player's stack size
    public void IncreaseStack()
    {
        playerStackSize++;
    }

    // Method to update the currency text in the UI
    public void UpdateCoinText()
    {
        currencyText.text = string.Format("Coins: {0}", playerCurrency);
    }

    // Method to update the player's outfit
    public void UpdatePlayerOutfit(Material shirtMaterial, Material pantsMaterial)
    {
        playerShirt.material = shirtMaterial; // Set the shirt material
        playerPants.material = pantsMaterial; // Set the pants material
    }
}
