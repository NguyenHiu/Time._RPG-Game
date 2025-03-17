using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amount;

    private void Start()
    {
        UpdateCurrency();
        PlayerManager.instance.OnCurrencyUpdated += UpdateCurrency;
    }

    private void UpdateCurrency()
    {
        amount.text = PlayerManager.instance.currency.ToString();
    }
}
