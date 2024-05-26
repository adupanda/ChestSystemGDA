using UnityEngine;

public class ChestModel
{
    public ChestType ChestType { get; private set; }
    public int UnlockTime { get; private set; } // in minutes
    public int RewardCoins { get; private set; }
    public int RewardGems { get; private set; }
    public bool IsCollected { get; private set; }

    public ChestModel(ChestData data)
    {
        Initialize(data);
    }

    private void Initialize(ChestData data)
    {
        ChestType = data.chestType;
        UnlockTime = data.unlockTime;
        GenerateRandomRewards(data.minRewardCoins, data.maxRewardCoins, data.minRewardGems, data.maxRewardGems);
    }

    private void GenerateRandomRewards(int minCoins, int maxCoins, int minGems, int maxGems)
    {
        RewardCoins = Random.Range(minCoins, maxCoins + 1);
        RewardGems = Random.Range(minGems, maxGems + 1);
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
