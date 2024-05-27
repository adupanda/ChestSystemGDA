using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChestView : MonoBehaviour
{

    [SerializeField] private Image chestImage;
    public Image ChestImage {  get { return chestImage; } }
   
    [SerializeField] private TextMeshProUGUI timerText;
    public TextMeshProUGUI TimerText { get { return timerText; } }
    [SerializeField] private TextMeshProUGUI gemCostText;
    public TextMeshProUGUI GemCostText { get { return gemCostText; } }

    [SerializeField] private Button startTimerButton;
    public Button StartTimerButton { get { return startTimerButton; } }
    [SerializeField] private Button unlockWithGemsButton;
    public Button UnlockWithGemsButton {  get { return unlockWithGemsButton; } }
    [SerializeField] private Button collectRewardsButton;
    public Button CollectRewardsButton { get { return collectRewardsButton; } }
    [SerializeField] private Button chestSlotButton;
    public Button ChestSlotButton {  get { return chestSlotButton; } }  

    private ChestController chestController;
    private bool unlockMenuOpen;

    

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

    

    
}
