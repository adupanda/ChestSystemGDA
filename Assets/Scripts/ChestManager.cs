using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public List<GameObject> chestSlots;
    public GameObject chestSlotPrefab;
    public Transform chestSlotContainer;

    public Chest bronzeChest;
    public Chest silverChest;
    public Chest goldChest;
    public Chest magicChest;

    private Queue<Chest> unlockQueue = new Queue<Chest>();
    [SerializeField]
    private Currency currency;
    [SerializeField]
    private CurrencyDisplay currencyDisplay;

    private void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < 4; i++)
        {
            var slot = Instantiate(chestSlotPrefab, chestSlotContainer);
            chestSlots.Add(slot);
        }
    }

    public void AddRandomChest()
    {
        foreach (var slot in chestSlots)
        {
            var chestSlot = slot.GetComponent<ChestSlot>();
            if (chestSlot.GetChest() == null)
            {
                Chest chest = InstantiateRandomChest();
                chest.Initialize(chest.chestType,chest.unlockTime,chest.minRewardCoins,chest.maxRewardCoins,chest.minRewardGems,chest.maxRewardGems);
                chestSlot.AssignChest(chest);
                chestSlot.InitializeCurrencyVariables(currency, currencyDisplay);
                break;
            }
        }
    }

    private Chest InstantiateRandomChest()
    {
        ChestType chestType = (ChestType)Random.Range(0, 4);

        switch (chestType)
        {
            case ChestType.Bronze:
                return Instantiate(bronzeChest);
            case ChestType.Silver:
                return Instantiate(silverChest);
            case ChestType.Gold:
                return Instantiate(goldChest);
            case ChestType.Magic:
                return Instantiate(magicChest);
            default:
                return null;
        }
    }

    public bool IsAnyChestUnlocking()
    {
        foreach (var slot in chestSlots)
        {
            var chestSlot = slot.GetComponent<ChestSlot>();
            if (chestSlot.GetChest() != null && chestSlot.GetChest().IsUnlocking())
            {
                return true;
            }
        }
        return false;
    }

    public void StartUnlockingChest(Chest chest)
    {
        chest.StartUnlocking();
    }

    public void QueueChestForUnlocking(Chest chest)
    {
        unlockQueue.Enqueue(chest);
    }

    public void ProcessUnlockQueue()
    {
        if (unlockQueue.Count > 0 && !IsAnyChestUnlocking())
        {
            Chest nextChest = unlockQueue.Dequeue();
            StartUnlockingChest(nextChest);
        }
    }
}
