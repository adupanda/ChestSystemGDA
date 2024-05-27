using UnityEngine;

public class ChestModel
{
    public ChestType ChestType { get; private set; }
    public int UnlockTime { get; private set; } // in minutes
    public int RewardCoins { get; private set; }
    public int RewardGems { get; private set; }
    public bool IsCollected { get; private set; }

    public Sprite ChestSprite { get; private set; } // Add this property

    public ChestModel(ChestData data)
    {
       
        ChestType = data.chestType;
        UnlockTime = data.unlockTime;
        ChestSprite = data.chestSprite;
        GenerateRandomRewards(data);
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
