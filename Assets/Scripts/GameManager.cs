using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    

    public int playerCurrency;
    public int playerStackSize = 3;

    public TMP_Text currencyText;

    public SkinnedMeshRenderer playerShirt;
    public SkinnedMeshRenderer playerPants;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        UpdateCoinText();
    }
    public void IncreaseStack()
    {         
        playerStackSize++;
    }
    public void UpdateCoinText()
    {
        currencyText.text = string.Format("Coins: {0}", playerCurrency);
    }

    public void UpdatePlayerOutfit(Material shirtMaterial, Material pantsMaterial)
    {
        playerShirt.material = shirtMaterial;
        playerPants.material = pantsMaterial;
    }

}
