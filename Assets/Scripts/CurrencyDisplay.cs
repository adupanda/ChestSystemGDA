using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;

    [SerializeField]
    private Currency currency;

    private void Start()
    {
       

        // Initially update the UI
        UpdateCurrencyUI();

        // Subscribe to the currency change event
        currency.OnCurrencyChanged += UpdateCurrencyUI;
    }

    public void UpdateCurrencyUI()
    {
        if (currency != null)
        {
            coinsText.text = "Coins: " + currency.coins.ToString();
            gemsText.text = "Gems: " + currency.gems.ToString();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the currency change event
        if (currency != null)
        {
            currency.OnCurrencyChanged -= UpdateCurrencyUI;
        }
    }
}
