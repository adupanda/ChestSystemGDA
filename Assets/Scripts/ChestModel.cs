using UnityEngine;

public class ChestModel
{
    public ChestType ChestType { get; private set; }
    public int UnlockTime { get; private set; } // in minutes
    public int RewardCoins { get; private set; }
    public int RewardGems { get; private set; }
    public bool IsCollected { get; private set; }

    public Sprite ChestSprite { get; private set; } // Add this property

    public int RemainingTime { get; private set; }
    public bool IsUnlocking { get; private set; }
    public bool IsUnlocked { get; private set; }

    public ChestModel(ChestData data)
    {

        ChestType = data.chestType;
        UnlockTime = data.unlockTime;
        ChestSprite = data.chestSprite; // Initialize the sprite
        GenerateRandomRewards(data);
        RemainingTime = UnlockTime * 60; // Initialize remaining time in seconds
        IsUnlocking = false;
        IsUnlocked = false;
        IsCollected = false;
    }

    public void StartUnlocking()
    {
        if (!IsUnlocking)
        {
            IsUnlocking = true;
            RemainingTime = UnlockTime * 60; // Reset remaining time in seconds
        }
    }

    public void UnlockInstantly()
    {
        RemainingTime = 0;
        IsUnlocking = false;
        IsUnlocked = true;
    }

    public void Tick()
    {
        if (IsUnlocking && RemainingTime > 0)
        {
            RemainingTime--;
            if (RemainingTime <= 0)
            {
                IsUnlocking = false;
                IsUnlocked = true;
            }
        }
    }

    private void GenerateRandomRewards(ChestData data)
    {
        RewardCoins = Random.Range(data.minRewardCoins, data.maxRewardCoins + 1);
        RewardGems = Random.Range(data.minRewardGems, data.maxRewardGems + 1);
    }

    public void CollectRewards(Currency currency)
    {
        if (!IsCollected)
        {
            currency.AddCoins(RewardCoins);
            currency.AddGems(RewardGems);
            IsCollected = true;
        }
    }
}
