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
        LockedGO.SetActive(!Accessory.IsUnlocked);
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
        Accessory.IsUnlocked = Currency.ModifyGameCurrencyAmount(-Accessory.UnlockCost);
        LockedGO.SetActive(!Accessory.IsUnlocked);
        return Accessory.IsUnlocked;
    }
    public bool TryUnlockWithAccessoryParts()
    {
        Accessory.IsUnlocked = Currency.ModifyGameCurrencyAmount(0, -Accessory.UnlockParts);
        LockedGO.SetActive(!Accessory.IsUnlocked);
        return Accessory.IsUnlocked;
    }
    public void OnSetAccessory()
    {
        if(UnlockGO.gameObject.activeSelf)
        {
            return;
        }
        if (Accessory.IsUnlocked)
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
