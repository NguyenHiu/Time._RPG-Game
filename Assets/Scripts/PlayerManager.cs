using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;
    public Action OnCurrencyUpdated;

    public int currency;
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);
    }

    public bool SpendCurrency(int _price)
    {
        if (currency < _price) 
            return false;

        currency -= _price;
        OnCurrencyUpdated?.Invoke();
        return true;
    }
}
