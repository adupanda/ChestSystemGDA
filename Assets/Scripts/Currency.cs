using UnityEngine;

public class Currency : MonoBehaviour
{
    public int coins = 500;
    public int gems = 500;

    public delegate void CurrencyChanged();
    public event CurrencyChanged OnCurrencyChanged;

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCurrencyChanged?.Invoke();
    }

    public void AddGems(int amount)
    {
        gems += amount;
        OnCurrencyChanged?.Invoke();
    }

    public void DeductGems(int amount)
    {
        if (gems >= amount)
        {
            gems -= amount;
            OnCurrencyChanged?.Invoke();
        }
    }
}
