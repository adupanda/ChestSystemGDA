using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI gemsText;

    [SerializeField]
    private Currency currency;

    
    private void OnEnable()
    {
        currency.OnCoinsChanged += UpdateCoinsUI;
        currency.OnGemsChanged += UpdateGemsUI;
    }

    private void OnDisable()
    {
        currency.OnCoinsChanged -= UpdateCoinsUI;
        currency.OnGemsChanged -= UpdateGemsUI;
    }


    public void UpdateCoinsUI(int amount)
    {
        coinsText.text = "Coins: " + amount.ToString();
    }

    public void UpdateGemsUI(int amount)
    {
        gemsText.text = "Gems: " +  amount.ToString();
    }

   
}
