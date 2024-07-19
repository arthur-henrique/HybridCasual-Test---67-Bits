using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingPad : MonoBehaviour
{
    public float activationTimer = 2f;
    [SerializeField] private Image timerImage;
    private Coroutine activationCoroutine;
    public GameObject OptionCanvas;
    public TMP_Text costText;
    private int currentCost = 10;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCostText(currentCost);
    }

    
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
            OptionCanvas.SetActive(false);
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
        OptionCanvas.SetActive(true);
    }

    public void UpdateCostText(int cost)
    {
        costText.text = string.Format("Cost: {0}", cost);
    }

    public void BuyStackUp()
    {
        if(GameManager.instance.playerCurrency >= currentCost)
        {
            GameManager.instance.playerCurrency -= currentCost;
            GameManager.instance.UpdateCoinText();
            currentCost += 10;
            GameManager.instance.IncreaseStack();
            UpdateCostText(currentCost);
        }
    }
}
