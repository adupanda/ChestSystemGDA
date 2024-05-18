using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
{
    public TextMeshProUGUI chestTypeText;
    public TextMeshProUGUI timerText;
    public Button startTimerButton;
    public Button unlockWithGemsButton;
    public Button collectRewardsButton;

    private Chest chest;
    private Currency currency;
    private CurrencyDisplay currencyDisplay;
    private ChestManager chestManager;

    private void Start()
    {
        chestManager = FindObjectOfType<ChestManager>();
        collectRewardsButton.onClick.AddListener(CollectRewards);

        // Hide the buttons initially
        startTimerButton.gameObject.SetActive(false);
        unlockWithGemsButton.gameObject.SetActive(false);
        collectRewardsButton.gameObject.SetActive(false );
    }

    public void InitializeCurrencyVariables(Currency currency, CurrencyDisplay currencyDisplay)
    {
        this.currency = currency;
        this.currencyDisplay = currencyDisplay;
    }

    public void AssignChest(Chest newChest)
    {
        if (chest != null)
        {
            chest.OnTimerUpdated -= UpdateTimerUI; // Unsubscribe from previous chest's event
        }

        chest = newChest;
        chestTypeText.text = newChest.chestType.ToString();
        startTimerButton.onClick.AddListener(StartUnlocking);
        unlockWithGemsButton.onClick.AddListener(UnlockWithGems);

        chest.OnTimerUpdated += UpdateTimerUI; // Subscribe to new chest's event
        UpdateTimerUI(chest.GetRemainingTime()); // Initial timer update

        collectRewardsButton.gameObject.SetActive(false); // Hide the collect button initially
    }

    private void Update()
    {
        if (chest != null && chest.IsUnlocked())
        {
            timerText.text = "Unlocked! Tap to collect.";
            collectRewardsButton.gameObject.SetActive(true); // Show the collect button when unlocked
        }
    }

    public void OnSlotClicked()
    {
        if (chest != null)
        {
            startTimerButton.gameObject.SetActive(true);
            unlockWithGemsButton.gameObject.SetActive(true);
            UpdateGemCostText();
        }
    }

    private void StartUnlocking()
    {
        if (chest != null && !chestManager.IsAnyChestUnlocking())
        {
            chestManager.StartUnlockingChest(chest);
            HideUnlockButtons();
        }
        else if (chest != null)
        {
            chestManager.QueueChestForUnlocking(chest);
            HideUnlockButtons();
        }
    }

    private void UnlockWithGems()
    {
        if (chest != null && currency != null)
        {
            int cost = chest.GetUnlockCost();
            if (currency.gems >= cost)
            {
                currency.DeductGems(cost);
                chest.UnlockWithGems(cost);
                currencyDisplay.UpdateCurrencyUI(); // Update the currency display
                HideUnlockButtons();
            }
            else
            {
                Debug.Log("Not enough gems to unlock the chest.");
            }
        }
    }

    private void UpdateGemCostText()
    {
        if (chest != null)
        {
            int cost = chest.GetUnlockCost();
            unlockWithGemsButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Unlock Now Cost: " + cost.ToString() + " gems");
        }
    }

    private void CollectRewards()
    {
        if (chest != null && chest.IsUnlocked() && !chest.IsCollected())
        {
            chest.CollectRewards(currency);
            currencyDisplay.UpdateCurrencyUI(); // Update the currency display
            ClearChestSlot();
            chestManager.ProcessUnlockQueue();
        }
    }

    public Chest GetChest()
    {
        return chest;
    }

    private void UpdateTimerUI(int remainingTime)
    {
        if (remainingTime > 0)
        {
            int minutes = remainingTime / 60;
            int seconds = remainingTime % 60;
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
        else
        {
            timerText.text = "";
        }
    }

    private void ClearChestSlot()
    {
        if (chest != null)
        {
            chest.OnTimerUpdated -= UpdateTimerUI; // Unsubscribe to avoid memory leaks
            chest = null;
        }

        chestTypeText.text = string.Empty;
        timerText.text = string.Empty;
        startTimerButton.onClick.RemoveAllListeners();
        unlockWithGemsButton.onClick.RemoveAllListeners();
        

        startTimerButton.gameObject.SetActive(false);
        unlockWithGemsButton.gameObject.SetActive(false);
        collectRewardsButton.gameObject.SetActive(false); // Hide the collect button
    }

    private void OnDestroy()
    {
        if (chest != null)
        {
            chest.OnTimerUpdated -= UpdateTimerUI; // Unsubscribe to avoid memory leaks
        }
    }

    private void HideUnlockButtons()
    {
        startTimerButton.gameObject.SetActive(false);
        unlockWithGemsButton.gameObject.SetActive(false);
    }
}
