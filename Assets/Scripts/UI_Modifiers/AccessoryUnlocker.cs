using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AccessoryUnlocker : MonoBehaviour
{
    public AccessoryUI ActiveUI;
    public Text CostText;
    public Text UseCurrencyButton;
    public Text UsePartsButton;
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (ActiveUI)
        {
            Accessory accessory = ActiveUI.Accessory;
            InGameCurrency currency = ActiveUI.Currency;
            Utils.Builder.Clear();
            Utils.Builder.AppendFormat("This accessory {0} costs either {1} currency or {2} accessory parts to unlock. You {3}have enough to unlock this.", accessory.Describer != null ? accessory.Describer.Name : accessory.name, accessory.PurchaseInfo.CurrencyCost, accessory.PurchaseInfo.AccessoryPartsCost, currency.CanModifyGameCurrency(-accessory.PurchaseInfo.CurrencyCost) || currency.CanModifyGameCurrency(0, -accessory.PurchaseInfo.AccessoryPartsCost) ? "" : "don't ");
            CostText.text = Utils.Builder.ToString();
            Utils.Builder.Clear();

            UseCurrencyButton.text = currency.CanModifyGameCurrency(-accessory.PurchaseInfo.CurrencyCost) ? "Enough currency" : "Not enough currency";
            UsePartsButton.text = currency.CanModifyGameCurrency(0, -accessory.PurchaseInfo.AccessoryPartsCost) ? "Enough parts" : "Not enough parts";
        }
    }
    public void BuyWithCurrency()
    {
        if (ActiveUI)
        {
            if (ActiveUI.TryUnlockWithCurrency())
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    public void BuyWithParts()
    {
        if (ActiveUI)
        {
            if (ActiveUI.TryUnlockWithAccessoryParts())
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
