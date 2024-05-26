using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChestView : MonoBehaviour
{
    public TextMeshProUGUI chestTypeText;
    [SerializeField] private TextMeshProUGUI timerText;


    
    [SerializeField] private Button startTimerButton;
    [SerializeField] private Button unlockWithGemsButton;
    [SerializeField] private Button collectRewardsButton;

    [SerializeField] private Button chestSlotButton;


    private ChestModel chestModel;
    private Currency currency;
    private CurrencyDisplay currencyDisplay;
    private int remainingTime;
    public bool isUnlocking;
    private bool isUnlocked;

    private bool unlockMenuOpen;

    public void Initialize(ChestModel model, Currency currency,CurrencyDisplay currencyDisplay)
    {
        this.chestModel = model;
        this.currency = currency;
        this.currencyDisplay = currencyDisplay;

        startTimerButton.onClick.AddListener(StartUnlocking);
        unlockWithGemsButton.onClick.AddListener(UnlockWithGems);
        collectRewardsButton.onClick.AddListener(CollectRewards);
        chestSlotButton.onClick.AddListener(ChestSlotClicked);

        SetChestType(chestModel.ChestType.ToString());
        
    }

    private void ChestSlotClicked()
    {
        if (!unlockMenuOpen)
        {
            ShowUnlockButtons();
            unlockMenuOpen = true;
        }
        else
        {
            HideUnlockButtons();
            unlockMenuOpen = false;
        }
    }

    public void SetChestType(string chestType)
    {
        chestTypeText.text = chestType;
    }

    public void ShowUnlockButtons()
    {
        startTimerButton.gameObject.SetActive(true);
        unlockWithGemsButton.gameObject.SetActive(true);
        
    }

    public void HideUnlockButtons()
    {
        startTimerButton.gameObject.SetActive(false);
        unlockWithGemsButton.gameObject.SetActive(false);
        
    }

    public void ShowCollectButton()
    {
        collectRewardsButton.gameObject.SetActive(true);
    }

    public void HideCollectButton()
    {
        collectRewardsButton.gameObject.SetActive(false);
    }

    public void StartUnlocking()
    {
        if (!isUnlocking)
        {
            isUnlocking = true;
            remainingTime = chestModel.UnlockTime * 60; // Convert to seconds
            StartCoroutine(UnlockTimer());
            HideUnlockButtons();
        }
    }

    private IEnumerator UnlockTimer()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            UpdateTimerUI(remainingTime);
        }

        isUnlocking = false;
        isUnlocked = true;
        UpdateTimerUI(remainingTime); // Final update when unlocked
        ShowCollectButton();
    }

    private void UnlockWithGems()
    {
        int cost = GetUnlockCost();
        if (currency.gems >= cost)
        {
            currency.DeductGems(cost);
            remainingTime = 0;
            isUnlocking = false;
            isUnlocked = true;
            UpdateTimerUI(remainingTime);
            ShowCollectButton();
            currencyDisplay.UpdateCurrencyUI(); // Update the currency display
            HideUnlockButtons();
        }
        else
        {
            Debug.Log("Not enough gems to unlock the chest.");
        }
    }

    private int GetUnlockCost()
    {
        if(isUnlocking)
        {
            return Mathf.CeilToInt(remainingTime / 20f); // 1 gem per 20 seconds
        }
        else
        {
            return Mathf.CeilToInt(chestModel.UnlockTime * 3f); // each minute is 3 intervals of 20 seconds so time in minutes multiplied by 3
        }
        // Calculate the gem cost to unlock instantly based on remaining time

        
    }

    private void UpdateTimerUI(int remainingTime)
    {
        if (remainingTime > 0)
        {
            int minutes = remainingTime / 60;
            int seconds = remainingTime % 60;
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
        else
        {
            timerText.text = "Unlocked!";
        }
    }

    private void CollectRewards()
    {
        if (isUnlocked && !chestModel.IsCollected)
        {
            chestModel.CollectRewards(currency);
            ClearChestSlot();
        }
    }

    public void ClearChestSlot()
    {
        chestTypeText.text = string.Empty;
        timerText.text = string.Empty;
        

        startTimerButton.onClick.RemoveAllListeners();
        unlockWithGemsButton.onClick.RemoveAllListeners();
        collectRewardsButton.onClick.RemoveAllListeners();

        HideUnlockButtons();
        HideCollectButton();
    }
}
