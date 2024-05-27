using System;
using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] private int coins = 500;
    public int COINS => coins;
    [SerializeField] private int gems = 500;
    public int GEMS => gems;

    

    public Action<int> OnCoinsChanged;
    public Action<int> OnGemsChanged;

    private void Start()
    {
        AddCoins(500);
        AddGems(500);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
    }

    public void AddGems(int amount)
    {
        gems += amount;
        OnGemsChanged?.Invoke(gems);
    }
}
