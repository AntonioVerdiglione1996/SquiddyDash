using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyTextHandler : MonoBehaviour
{
    public InGameCurrency Currency;

    public TextMeshProUGUI Text;

    private long lastCurrency = 0;

    private void OnEnable()
    {
        if (!Text)
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
        Update();
    }
    public void Update()
    {
        if (!Text)
        {
            return;
        }

        long curr = Currency.GameCurrency;

        if (curr != lastCurrency)
        {
            lastCurrency = curr;
            Text.text = lastCurrency.ToString();

        }
    }
}
