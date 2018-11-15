using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyTextHandler : MonoBehaviour
{
    public InGameCurrency CurrencySystem;
    public TextMeshProUGUI Text;
    void Update()
    {
        Text.text = CurrencySystem.Currency.ToString();
    }
}
