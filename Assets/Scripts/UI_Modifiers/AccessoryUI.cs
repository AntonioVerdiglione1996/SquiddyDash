using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryUI : DescriberUI
{
    public int AccessoryIndex = 0;
    public StoringCurrentModelToSpawn Scm;
    public Button[] Buttons = new Button[0];
    public bool GetChildButtons = true;
    public Accessory Accessory { get; private set; }
    public AccessoryUnlocker UnlockGO;
    public GameObject LockedGO;
    public InGameCurrency Currency;
    // Use this for initialization
    public void OnValidate()
    {
        if (GetChildButtons)
        {
            Buttons = GetComponentsInChildren<Button>(true);
        }
    }
    public virtual void SetAccessory(Accessory accessory, int index)
    {
        Accessory = accessory;
        AccessoryIndex = index;
        UnlockGO.gameObject.SetActive(false);
        LockedGO.SetActive(!Accessory.PurchaseInfo.IsPurchased);
        if (Buttons != null)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Button button = Buttons[i];
                button.onClick.RemoveListener(OnSetAccessory);
                button.onClick.AddListener(OnSetAccessory);
            }
        }
    }
    public bool TryUnlockWithCurrency()
    {
        Accessory.PurchaseInfo.IsPurchased = Currency.ModifyGameCurrencyAmount(-Accessory.PurchaseInfo.CurrencyCost);
        LockedGO.SetActive(!Accessory.PurchaseInfo.IsPurchased);
        return Accessory.PurchaseInfo.IsPurchased;
    }
    public bool TryUnlockWithAccessoryParts()
    {
        Accessory.PurchaseInfo.IsPurchased = Currency.ModifyGameCurrencyAmount(0, -Accessory.PurchaseInfo.AccessoryPartsCost);
        LockedGO.SetActive(!Accessory.PurchaseInfo.IsPurchased);
        return Accessory.PurchaseInfo.IsPurchased;
    }
    public void OnSetAccessory()
    {
        if(UnlockGO.gameObject.activeSelf)
        {
            return;
        }
        if (Accessory.PurchaseInfo.IsPurchased)
        {
            Scm.AddAccessory(AccessoryIndex);
        }
        else
        {
            UnlockGO.ActiveUI = this;
            UnlockGO.gameObject.SetActive(true);
        }
    }
}
