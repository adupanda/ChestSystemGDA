using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private List<ChestView> chestSlots = new List<ChestView>();
    [SerializeField] private GameObject chestSlotPrefab;
    [SerializeField] private Transform chestSlotContainer;

    [SerializeField] private ChestData bronzeChestData;
    [SerializeField] private ChestData silverChestData;
    [SerializeField] private ChestData goldChestData;
    [SerializeField] private ChestData magicChestData;

    private Queue<ChestController> unlockQueue = new Queue<ChestController>();
    [SerializeField] private Currency currency;
    [SerializeField] private CurrencyDisplay currencyDisplay;

    private void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < 4; i++)
        {
            var slot = Instantiate(chestSlotPrefab, chestSlotContainer).GetComponent<ChestView>();
            slot.HideUnlockButtons();
            slot.HideCollectButton();
            chestSlots.Add(slot);
        }
    }

    public void AddRandomChest()
    {
        foreach (var slot in chestSlots)
        {
            if (slot != null && string.IsNullOrEmpty(slot.chestTypeText.text))
            {
                ChestData chestData = GetRandomChestData();
                ChestModel chestModel = new ChestModel(chestData);
                ChestController chestController = new ChestController(chestModel, slot, currency,currencyDisplay);
                slot.HideUnlockButtons();
                slot.HideCollectButton();
                break;
            }
        }
    }

    private ChestData GetRandomChestData()
    {
        ChestType chestType = (ChestType)Random.Range(0, 4);
        switch (chestType)
        {
            case ChestType.Bronze:
                return bronzeChestData;
            case ChestType.Silver:
                return silverChestData;
            case ChestType.Gold:
                return goldChestData;
            case ChestType.Magic:
                return magicChestData;
            default:
                return null;
        }
    }

    public bool IsAnyChestUnlocking()
    {
        foreach (var slot in chestSlots)
        {
            if (slot != null && !string.IsNullOrEmpty(slot.chestTypeText.text))
            {
                var chestController = slot.GetComponent<ChestController>();
                if (chestController != null && chestController.IsUnlocking())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartUnlockingChest(ChestController chestController)
    {
        chestController.StartUnlocking();
    }

    public void QueueChestForUnlocking(ChestController chestController)
    {
        unlockQueue.Enqueue(chestController);
    }

    public void ProcessUnlockQueue()
    {
        if (unlockQueue.Count > 0 && !IsAnyChestUnlocking())
        {
            ChestController nextChest = unlockQueue.Dequeue();
            StartUnlockingChest(nextChest);
        }
    }
}
