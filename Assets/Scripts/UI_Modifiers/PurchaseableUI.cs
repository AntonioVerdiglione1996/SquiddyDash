using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[System.Serializable]
public class IntEvent : UnityEvent<int, IPurchaseObject> { }
public class PurchaseableUI : MonoBehaviour
{
    public IPurchaseObject PurchaseObj { get; protected set; }
    public InGameCurrency Currency;
    public PurchaseableUIUnlocker UnlockGO;
    public RectTransform LockedGO;
    public IntEvent OnPurchasedEvent;
    public IntEvent OnClickObjPurchasedEvent;
    public IntEvent OnClickObjNotPurchasedEvent;

    public SimpleAudioEvent FailedBuy;

    public CustomButton CustomButton;
    public Button Button;

    public DescriberUI DescriberUI;

    public bool UseNormalButton = true;

    public int Index;


    private void Awake()
    {
        if (LockedGO)
        {
            LockedGO.gameObject.SetActive(false);
        }
        if (UnlockGO)
        {
            UnlockGO.gameObject.SetActive(false);
        }
        OnPurchasedEvent.AddListener(OnPurchased);
    }
    private void OnDestroy()
    {
        OnPurchasedEvent.RemoveListener(OnPurchased);
    }
    private void OnEnable()
    {
        if (!UseNormalButton && CustomButton)
        {
            CustomButton.OnClickDownEvent.AddListener(OnClickDown);
        }
        else if (Button)
        {
            Button.onClick.AddListener(OnClickDown);
        }
    }
    private void OnDisable()
    {
        if (CustomButton)
        {
            CustomButton.OnClickDownEvent.RemoveListener(OnClickDown);
        }
        if (Button)
        {
            Button.onClick.RemoveListener(OnClickDown);
        }
    }
    public virtual bool TryUnlockWithCurrency()
    {
        return Unlock(PurchaseObj.PurchaseInfo.CurrencyCost, 0, 0);
    }
    public virtual bool TryUnlockWithAccessoryParts()
    {
        return Unlock(0, PurchaseObj.PurchaseInfo.AccessoryPartsCost, 0);
    }
    public virtual bool TryUnlockWithSkinParts()
    {
        return Unlock(0, 0, PurchaseObj.PurchaseInfo.SkinPartsCost);
    }

    private bool Unlock(int currencyCost, int accCost, int skinCost)
    {
        if (PurchaseObj.IsPurchased || currencyCost < 0 || accCost < 0 || skinCost < 0)
        {
            return false;
        }
        PurchaseObj.IsPurchased = Currency.ModifyGameCurrencyAmount(-currencyCost, -accCost, -skinCost);
        if (PurchaseObj.IsPurchased)
        {
            OnPurchasedEvent.Invoke(Index, PurchaseObj);
            return true;
        }
        return false;
    }

    public virtual bool SetPurchaseable(IPurchaseObject obj, PurchaseableUIUnlocker unlocker, int Index)
    {
        PurchaseObj = obj;
        UnlockGO = unlocker;
        this.Index = Index;
        if (DescriberUI)
        {
            DescriberUI.SetDescriber(PurchaseObj == null ? null : PurchaseObj.Describer);
        }
        if (UnlockGO)
        {
            UnlockGO.gameObject.SetActive(false);
        }
        bool locked = PurchaseObj == null ? true : !PurchaseObj.IsPurchased;
        if (LockedGO)
        {
            LockedGO.gameObject.SetActive(locked);
        }
        return locked;
    }
    protected virtual void OnClickDown()
    {
        if (UnlockGO && UnlockGO.gameObject.activeSelf)
        {
            return;
        }
        bool unlocked = PurchaseObj == null ? true : PurchaseObj.IsPurchased;
        if (UnlockGO)
        {
            UnlockGO.SetPurchaseableUI(this);
            UnlockGO.gameObject.SetActive(!unlocked);
        }
        if (unlocked)
        {
            OnClickObjPurchasedEvent.Invoke(Index, PurchaseObj);
        }
        else
        {
            OnClickObjNotPurchasedEvent.Invoke(Index, PurchaseObj);
        }
    }
    protected virtual void OnPurchased(int Index, IPurchaseObject obj)
    {
        if (LockedGO)
        {
            LockedGO.gameObject.SetActive(false);
        }
        if (UnlockGO)
        {
            UnlockGO.gameObject.SetActive(false);
        }
    }
}
