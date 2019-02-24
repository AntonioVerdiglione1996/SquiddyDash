using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CurrencyTextHandler : MonoBehaviour
{
    public InGameCurrency Currency;

    public TextMeshProUGUI CurrencyText;
    public TextMeshProUGUI SkinPartsText;
    public TextMeshProUGUI AccessoryPartsText;

    public Text NormalCurrencyText;
    public Text NormalSkinPartsText;
    public Text NormalAccessoryPartsText;

    private int lastCurrency = 0;
    private int lastSkinParts = 0;
    private int lastAccessoryParts = 0;

    private void OnEnable()
    {
        Update();
    }
    public void Update()
    {
        if (CurrencyText || NormalCurrencyText)
        {
            if (Currency.GameCurrency != lastCurrency)
            {
                lastCurrency = Currency.GameCurrency;
                string txt = lastCurrency.ToString();
                if (CurrencyText)
                {
                    CurrencyText.text = txt;
                }
                if (NormalCurrencyText)
                {
                    NormalCurrencyText.text = txt;
                }
            }
        }
        if (SkinPartsText || NormalSkinPartsText)
        {
            if (Currency.SkinParts != lastSkinParts)
            {
                lastSkinParts = Currency.SkinParts;
                string txt = lastSkinParts.ToString();
                if (SkinPartsText)
                {
                    SkinPartsText.text = txt;
                }
                if (NormalSkinPartsText)
                {
                    NormalSkinPartsText.text = txt;
                }
            }
        }
        if (AccessoryPartsText || NormalAccessoryPartsText)
        {
            if (Currency.AccessoryParts != lastAccessoryParts)
            {
                lastAccessoryParts = Currency.AccessoryParts;
                string txt = lastAccessoryParts.ToString();
                if (AccessoryPartsText)
                {
                    AccessoryPartsText.text = txt;
                }
                if (NormalAccessoryPartsText)
                {
                    NormalAccessoryPartsText.text = txt;
                }
            }
        }
    }
}
