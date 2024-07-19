using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeClothesButton : MonoBehaviour
{
    public bool isLocked = true;
    public Material shirtMaterial;
    public Material pantsMaterial;

    // UI Elements
    public GameObject priceGroup;
    public TMP_Text priceText;

    public int cost = 1;

    private void Start()
    {
        if (isLocked)
        {
            priceGroup.SetActive(true);
            priceText.text = string.Format("Cost: {0}", cost);
        }
        else
        {
            priceGroup.SetActive(false);
        }
    }
    public void OnButtonPress()
    {
        if(isLocked)
        {
            if (GameManager.instance.playerCurrency >= cost)
            {
                isLocked = false;
                priceGroup.SetActive(false);
                GameManager.instance.playerCurrency -= cost;
                GameManager.instance.UpdateCoinText();
                GameManager.instance.UpdatePlayerOutfit(shirtMaterial, pantsMaterial);
            }
        }
        else
        {
            GameManager.instance.UpdatePlayerOutfit(shirtMaterial, pantsMaterial);
        }

    }
}
