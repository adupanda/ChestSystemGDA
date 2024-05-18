using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ChestType
{
    Bronze,
    Silver,
    Gold,
    Magic
}


[CreateAssetMenu(fileName = "NewChest", menuName = "Chest")]
public class Chest : ScriptableObject
{
    public ChestType chestType;
    public int unlockTime; // in minutes
    public int minRewardCoins;
    public int maxRewardCoins;
    public int minRewardGems;
    public int maxRewardGems;

    private int rewardCoins;
    private int rewardGems;
    private int remainingTime;
    private bool isUnlocking;
    private bool isUnlocked;
    private bool isCollected;

    public delegate void TimerUpdated(int remainingTime);
    public event TimerUpdated OnTimerUpdated;

    public void Initialize(ChestType chestType, int unlockTime, int minRewardCoins, int maxRewardCoins, int minRewardGems, int maxRewardGems)
    {
        this.chestType = chestType;
        this.unlockTime = unlockTime;
        this.minRewardCoins = minRewardCoins;
        this.maxRewardCoins = maxRewardCoins;
        this.minRewardGems = minRewardGems;
        this.maxRewardGems = maxRewardGems;
        GenerateRandomRewards();
    }
    private void GenerateRandomRewards()
    {
        rewardCoins = Random.Range(minRewardCoins, maxRewardCoins + 1);
        rewardGems = Random.Range(minRewardGems, maxRewardGems + 1);
    }
    public void StartUnlocking()
    {
        if (!isUnlocking)
        {
            isUnlocking = true;
            remainingTime = unlockTime * 60; // Convert to seconds
            CoroutineRunner.Instance.StartCoroutine(UnlockTimer());
        }
    }

    private IEnumerator UnlockTimer()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            OnTimerUpdated?.Invoke(remainingTime); // Notify about the timer update
        }

        isUnlocking = false;
        isUnlocked = true;
        OnTimerUpdated?.Invoke(remainingTime); // Final update when unlocked
    }

    public void UnlockWithGems(int gems)
    {
        int requiredGems = GetUnlockCost();
        if (gems >= requiredGems)
        {
            remainingTime = 0;
            isUnlocking = false;
            isUnlocked = true;
            OnTimerUpdated?.Invoke(remainingTime); // Notify about the timer update
        }
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public bool IsCollected()
    {
        return isCollected;
    }

    public void CollectRewards(Currency currency)
    {
        if (isUnlocked && !isCollected)
        {
            currency.AddCoins(rewardCoins);
            currency.AddGems(rewardGems);
            isCollected = true;
        }
    }

    public int GetRemainingTime()
    {
        return remainingTime;
    }

    public bool IsUnlocking()
    {
        return isUnlocking;
    }

    public int GetUnlockCost()
    {
        if (IsUnlocking())
        {
            // Calculate the gem cost to unlock instantly based on remaining time
            return Mathf.CeilToInt(remainingTime / 20f); // 1 gem per 20 seconds
        }
        else
        {
            return Mathf.CeilToInt((unlockTime * 60) / 20f);
        }
       
    }
}

