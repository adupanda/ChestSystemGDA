using UnityEngine;





[CreateAssetMenu(fileName = "NewChest", menuName = "Chest")]
public class ChestData : ScriptableObject
{
    public ChestType chestType;
    public Sprite chestSprite;
    public int unlockTime; // in minutes
    public int minRewardCoins;
    public int maxRewardCoins;
    public int minRewardGems;
    public int maxRewardGems;
}
