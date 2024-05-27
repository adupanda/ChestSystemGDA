using System;
using System.Collections;
using UnityEngine;

public class ChestController
{
    private ChestModel chestModel;
    private ChestView chestView;
    private Currency currency;
    private CurrencyDisplay currencyDisplay;
    private int remainingTime;
    private bool isUnlocking;
    private bool isUnlocked;

    public Action<int> OnTimerUpdated;

    public Action<int> OnGemCostUpdated;

    public ChestController(ChestData data, ChestView view, Currency currency, CurrencyDisplay currencyDisplay)
    {
        this.chestModel = new ChestModel(data); 
        this.chestView = view;
        this.currency = currency;
        this.currencyDisplay = currencyDisplay;

        chestView.Initialize(this);
    }

    public void StartUnlocking()
    {
        if (!isUnlocking)
        {
            isUnlocking = true;
            remainingTime = chestModel.UnlockTime * 60; // Convert to seconds
            chestView.StartCoroutine(UnlockTimer());
            chestView.HideUnlockButtons();
        }
    }

    private IEnumerator UnlockTimer()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            OnTimerUpdated?.Invoke(remainingTime);
        }

        isUnlocking = false;
        isUnlocked = true;
        OnTimerUpdated?.Invoke(remainingTime);
        chestView.ShowCollectButton();
    }

    public void UnlockWithGems()
    {
        int cost = GetUnlockCost();
        if (currency.GEMS >= cost)
        {
            currency.AddGems(-cost);
            remainingTime = 0;
            isUnlocking = false;
            isUnlocked = true;
            OnTimerUpdated?.Invoke(remainingTime);
            chestView.ShowCollectButton();
            currencyDisplay.UpdateGemsUI(currency.GEMS); // Update the currency display
            chestView.HideUnlockButtons();
        }
        else
        {
            Debug.Log("Not enough gems to unlock the chest.");
        }
    }

    public int GetUnlockCost()
    {
        int cost;
        if (isUnlocking)
        {
            cost = Mathf.CeilToInt(remainingTime / 20f); // 1 gem per 20 seconds
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
        if (isUnlocked && !chestModel.IsCollected)
        {
            chestModel.CollectRewards(currency);
            chestView.ClearChestSlot();
        }
    }

    public bool IsUnlocking()
    {
        return isUnlocking;
    }

    public string GetChestType()
    {
        return chestModel.ChestType.ToString();
    }

    public Sprite GetChestSprite()
    {
        return chestModel.ChestSprite; // Ensure ChestModel exposes the sprite
    }

}
