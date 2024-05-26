using UnityEngine;

public enum ChestType
{
    Bronze,
    Silver,
    Gold,
    Magic
}



[CreateAssetMenu(fileName = "NewChest", menuName = "Chest")]
public class ChestData : ScriptableObject
{
    public ChestType chestType;
    public int unlockTime; // in minutes
    public int minRewardCoins;
    public int maxRewardCoins;
    public int minRewardGems;
    public int maxRewardGems;
}
