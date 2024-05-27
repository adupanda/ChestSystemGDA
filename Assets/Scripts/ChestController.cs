using System;
using System.Collections;
using UnityEngine;

public class ChestController
{
    private ChestModel chestModel;
    private ChestView chestView;
    private Currency currency;
    private CurrencyDisplay currencyDisplay;
    private bool unlockMenuOpen;
    private bool collectRewardsActive;


    public Action<int> OnGemCostUpdated;

    public ChestController(ChestData data, ChestView view, Currency currency, CurrencyDisplay currencyDisplay)
    {
        this.chestModel = new ChestModel(data); 
        this.chestView = view;
        this.currency = currency;
        this.currencyDisplay = currencyDisplay;

        InitializeView();
    }

    private void InitializeView()
    {
        chestView.StartTimerButton.onClick.AddListener(StartUnlocking);
        chestView.UnlockWithGemsButton.onClick.AddListener(UnlockWithGems);
        chestView.CollectRewardsButton.onClick.AddListener(CollectRewards);
        chestView.ChestSlotButton.onClick.AddListener(ChestSlotClicked);
        
        chestView.ChestImage.sprite = chestModel.ChestSprite;
        
        OnGemCostUpdated += UpdateGemCostText;
        unlockMenuOpen = false;
        HideUnlockButtons();
        HideCollectButton();
        ImageView(true);

        OnGemCostUpdated += chestView.UpdateGemCostText;
        
    }

    public void ClearChestSlot()
    {
        chestView.ChestImage.sprite = null;
        chestView.TimerText.text = string.Empty;
        chestView.GemCostText.text = string.Empty;

        chestView.StartTimerButton.onClick.RemoveAllListeners();
        chestView.UnlockWithGemsButton.onClick.RemoveAllListeners();
        chestView.CollectRewardsButton.onClick.RemoveAllListeners();
        chestView.ChestSlotButton.onClick.RemoveAllListeners();
        HideUnlockButtons();
        HideCollectButton();
    }

    private void ChestSlotClicked()
    {
        if(collectRewardsActive)
        {
            return;
        }
        if (!unlockMenuOpen)
        {
            ShowUnlockButtons();
            UpdateGemCostText(GetUnlockCost());
            unlockMenuOpen = true;
            
        }
        else
        {
            HideUnlockButtons();
            unlockMenuOpen = false;
        }
    }

    public void UpdateGemCostText(int cost)
    {
        chestView.GemCostText.text = "Cost: " + cost + " gems";
    }

    public void ImageView(bool isImageActive)
    {
        chestView.ChestImage.gameObject.SetActive(isImageActive);
    }

    public void ShowUnlockButtons()
    {
        chestView.StartTimerButton.gameObject.SetActive(true);
        chestView.UnlockWithGemsButton.gameObject.SetActive(true);
        unlockMenuOpen = true;
    }

    public void HideUnlockButtons()
    {
        chestView.StartTimerButton.gameObject.SetActive(false);
        chestView.UnlockWithGemsButton.gameObject.SetActive(false);
        unlockMenuOpen = false;
    }

    public void ShowCollectButton()
    {
        chestView.CollectRewardsButton.gameObject.SetActive(true);
        collectRewardsActive= true;
        ImageView(false);
    }

    public void HideCollectButton()
    {
        chestView.CollectRewardsButton.gameObject.SetActive(false);
        collectRewardsActive = false;
    }


    public void StartUnlocking()
    {
        if (!chestModel.IsUnlocking)
        {
            
            chestModel.StartUnlocking();
            chestView.StartCoroutine(UnlockTimer());
            
            HideUnlockButtons();
        }
    }

    private IEnumerator UnlockTimer()
    {
        while (chestModel.RemainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            chestModel.Tick();
            chestView.UpdateTimerUI(chestModel.RemainingTime);
        }


        HideUnlockButtons();
        ShowCollectButton();
    }

    

    public void UnlockWithGems()
    {
        int cost = GetUnlockCost();
        if (currency.GEMS >= cost)
        {
            currency.AddGems(-cost);
            chestModel.UnlockInstantly();

            chestView.UpdateTimerUI(chestModel.RemainingTime);
            ShowCollectButton();
            currencyDisplay.UpdateGemsUI(currency.GEMS); // Update the currency display
            HideUnlockButtons();
        }
        else
        {
            Debug.Log("Not enough gems to unlock the chest.");
        }
    }

    public int GetUnlockCost()
    {
        int cost;
        if (chestModel.IsUnlocking)
        {
            cost = Mathf.CeilToInt(chestModel.RemainingTime / 20f); // 1 gem per 20 seconds
        }
        else
        {
            cost = Mathf.CeilToInt(chestModel.UnlockTime * 3f); // each minute is 3 intervals of 20 seconds
        }
        OnGemCostUpdated?.Invoke(cost);
        return cost;
        
    }

    public void CollectRewards()
    {
        if (chestModel.IsUnlocked && !chestModel.IsCollected)
        {
            chestModel.CollectRewards(currency);
            ClearChestSlot();
        }
    }

    public bool IsUnlocking()
    {
        return chestModel.IsUnlocking;
    }

    public string GetChestType()
    {
        return chestModel.ChestType.ToString();
    }

    

}
