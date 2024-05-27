using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChestView : MonoBehaviour
{

    [SerializeField] public Image chestImage;
   
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI gemCostText;

    [SerializeField] private Button startTimerButton;
    [SerializeField] private Button unlockWithGemsButton;
    [SerializeField] private Button collectRewardsButton;
    [SerializeField] private Button chestSlotButton;

    private ChestController chestController;
    private bool unlockMenuOpen;

    public void Initialize(ChestController controller)
    {
        chestController = controller;

        startTimerButton.onClick.AddListener(chestController.StartUnlocking);
        unlockWithGemsButton.onClick.AddListener(chestController.UnlockWithGems);
        collectRewardsButton.onClick.AddListener(chestController.CollectRewards);
        chestSlotButton.onClick.AddListener(ChestSlotClicked);

        chestController.OnTimerUpdated += UpdateTimerUI;
        chestController.OnGemCostUpdated += UpdateGemCostText;
        unlockMenuOpen = false;
        SetChestSprite(chestController.GetChestSprite());
        HideUnlockButtons();
        HideCollectButton();
        ImageView(true);

    }

    private void ChestSlotClicked()
    {
        if (!unlockMenuOpen)
        {
            ShowUnlockButtons();
            UpdateGemCostText(chestController.GetUnlockCost());
            unlockMenuOpen = true;
        }
        else
        {
            HideUnlockButtons();
            unlockMenuOpen = false;
        }
    }

    public void ImageView(bool isImageActive)
    {
        chestImage.gameObject.SetActive(isImageActive);
    }
    
    public void ShowUnlockButtons()
    {
        startTimerButton.gameObject.SetActive(true);
        unlockWithGemsButton.gameObject.SetActive(true);
    }

    public void HideUnlockButtons()
    {
        startTimerButton.gameObject.SetActive(false);
        unlockWithGemsButton.gameObject.SetActive(false);
    }

    public void ShowCollectButton()
    {
        collectRewardsButton.gameObject.SetActive(true);
        ImageView(false);
    }

    public void HideCollectButton()
    {
        collectRewardsButton.gameObject.SetActive(false);
    }

    public void UpdateTimerUI(int remainingTime)
    {
        if (remainingTime > 0)
        {
            int minutes = remainingTime / 60;
            int seconds = remainingTime % 60;
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
        else
        {
            timerText.text = "Unlocked!";
        }
    }

    public void UpdateGemCostText(int cost)
    {
        gemCostText.text = "Cost: " + cost + " gems";
    }

    public void ClearChestSlot()
    {
        chestImage.sprite = null;
        timerText.text = string.Empty;
        gemCostText.text = string.Empty;

        startTimerButton.onClick.RemoveAllListeners();
        unlockWithGemsButton.onClick.RemoveAllListeners();
        collectRewardsButton.onClick.RemoveAllListeners();
        chestSlotButton.onClick.RemoveAllListeners();
        HideUnlockButtons();
        HideCollectButton();
    }

    public void SetChestSprite(Sprite chestSprite)
    {
        chestImage.sprite = chestSprite;
    }
}
