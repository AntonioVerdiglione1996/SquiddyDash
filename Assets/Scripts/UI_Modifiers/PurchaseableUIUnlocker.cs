using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
[System.Serializable]
public class PurchaseEvent : UnityEvent<PurchaseableUI> { }
public class PurchaseableUIUnlocker : MonoBehaviour
{
    public const string Cost = "Cost: ";
    public const string Curr = "";
    public const string Acc = "";
    public const string Skin = "";
    public const string NoCost = " Not available";
    public PurchaseableUI Ui { get; protected set; }
    public DescriberUI DescriberUI;
    public Button BackButton;
    public CustomButton CustomBackButton;

    public Button CurrencyButton;
    public Button AccPartsButton;
    public Button SkinPartsButton;
    public CustomButton CustomCurrencyButton;
    public CustomButton CustomAccPartsButton;
    public CustomButton CustomSkinPartsButton;

    public bool UseNormalButton = true;

    public AudioSource FailedToBuySource;
    public PurchaseEvent OnBuyFailedEvent;
    private void Awake()
    {
        Deactivate();
    }
    private void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (UseNormalButton && BackButton)
        {
            BackButton.onClick.AddListener(Deactivate);
        }
        else if (CustomBackButton)
        {
            CustomBackButton.OnClickDownEvent.AddListener(Deactivate);
        }

        Text txt1 = null;
        Text txt2 = null;
        Text txt3 = null;

        if (UseNormalButton && CurrencyButton)
        {
            CurrencyButton.onClick.AddListener(BuyWithCurrency);
            if (Ui && Ui.PurchaseObj != null && Ui.PurchaseObj.PurchaseInfo != null)
            {
                txt1 = CurrencyButton.GetComponentInChildren<Text>(true);
                if (txt1)
                {
                    txt1.text = Cost + (Ui.PurchaseObj.PurchaseInfo.CurrencyCost >= 0 ? Ui.PurchaseObj.PurchaseInfo.CurrencyCost + Curr : NoCost);
                }
            }
        }
        else if (CustomCurrencyButton)
        {
            CustomCurrencyButton.OnClickDownEvent.AddListener(BuyWithCurrency);
            if (Ui && Ui.PurchaseObj != null && Ui.PurchaseObj.PurchaseInfo != null)
            {
                Text txt = CustomCurrencyButton.GetComponentInChildren<Text>(true);
                if (txt && txt != txt1)
                {
                    txt.text = Cost + (Ui.PurchaseObj.PurchaseInfo.CurrencyCost >= 0 ? Ui.PurchaseObj.PurchaseInfo.CurrencyCost + Curr : NoCost);
                }
            }
        }

        if (UseNormalButton && AccPartsButton)
        {
            AccPartsButton.onClick.AddListener(BuyWithAccessoryParts);
            if (Ui && Ui.PurchaseObj != null && Ui.PurchaseObj.PurchaseInfo != null)
            {
                txt2 = AccPartsButton.GetComponentInChildren<Text>(true);
                if (txt2)
                {
                    txt2.text = Cost + (Ui.PurchaseObj.PurchaseInfo.AccessoryPartsCost >= 0 ? Ui.PurchaseObj.PurchaseInfo.AccessoryPartsCost + Acc : NoCost);
                }
            }
        }
        else if (CustomAccPartsButton)
        {
            CustomAccPartsButton.OnClickDownEvent.AddListener(BuyWithAccessoryParts);
            if (Ui && Ui.PurchaseObj != null && Ui.PurchaseObj.PurchaseInfo != null)
            {
                Text txt = CustomAccPartsButton.GetComponentInChildren<Text>(true);
                if (txt && txt != txt2)
                {
                    txt.text = Cost + (Ui.PurchaseObj.PurchaseInfo.AccessoryPartsCost >= 0 ? Ui.PurchaseObj.PurchaseInfo.AccessoryPartsCost + Acc : NoCost);
                }
            }
        }

        if (UseNormalButton && SkinPartsButton)
        {
            SkinPartsButton.onClick.AddListener(BuyWithSkinParts);
            if (Ui && Ui.PurchaseObj != null && Ui.PurchaseObj.PurchaseInfo != null)
            {
                txt3 = SkinPartsButton.GetComponentInChildren<Text>(true);
                if (txt3)
                {
                    txt3.text = Cost + (Ui.PurchaseObj.PurchaseInfo.SkinPartsCost >= 0 ? Ui.PurchaseObj.PurchaseInfo.SkinPartsCost + Skin : NoCost);
                }
            }
        }      
        else if (CustomSkinPartsButton)
        {
            CustomSkinPartsButton.OnClickDownEvent.AddListener(BuyWithSkinParts);
            if (Ui && Ui.PurchaseObj != null && Ui.PurchaseObj.PurchaseInfo != null)
            {
                Text txt = CustomSkinPartsButton.GetComponentInChildren<Text>(true);
                if (txt && txt != txt3)
                {
                    txt.text = Cost + (Ui.PurchaseObj.PurchaseInfo.SkinPartsCost >= 0 ? Ui.PurchaseObj.PurchaseInfo.SkinPartsCost + Skin : NoCost);
                }
            }
        }
    }
    private void OnDisable()
    {
        if (BackButton)
        {
            BackButton.onClick.RemoveListener(Deactivate);
        }
        if (CustomBackButton)
        {
            CustomBackButton.OnClickDownEvent.RemoveListener(Deactivate);
        }
        if (CurrencyButton)
        {
            CurrencyButton.onClick.RemoveListener(BuyWithCurrency);
        }
        if (AccPartsButton)
        {
            AccPartsButton.onClick.RemoveListener(BuyWithAccessoryParts);
        }
        if (SkinPartsButton)
        {
            SkinPartsButton.onClick.RemoveListener(BuyWithSkinParts);
        }
        if (CustomCurrencyButton)
        {
            CustomCurrencyButton.OnClickDownEvent.RemoveListener(BuyWithCurrency);
        }
        if (CustomAccPartsButton)
        {
            CustomAccPartsButton.OnClickDownEvent.RemoveListener(BuyWithAccessoryParts);
        }
        if (CustomSkinPartsButton)
        {
            CustomSkinPartsButton.OnClickDownEvent.RemoveListener(BuyWithSkinParts);
        }
    }
    public virtual void SetPurchaseableUI(PurchaseableUI Ui)
    {
        this.Ui = Ui;
        if (DescriberUI)
        {
            DescriberUI.SetDescriber(Ui == null || Ui.PurchaseObj == null ? null : Ui.PurchaseObj.Describer);
        }
    }
    public void BuyWithCurrency()
    {
        if (Ui)
        {
            Ui.TryUnlockWithCurrency();
        }
    }
    public void BuyWithAccessoryParts()
    {
        if (Ui)
        {
            Ui.TryUnlockWithAccessoryParts();
        }
    }
    public void BuyWithSkinParts()
    {
        if (Ui)
        {
            if (!Ui.TryUnlockWithSkinParts())
            {
                OnBuyFailedEvent.Invoke(Ui);
                if (Ui.FailedBuy)
                {
                    if (FailedToBuySource)
                    {
                        Ui.FailedBuy.Play(FailedToBuySource);
                    }
                    else
                    {
                        Ui.FailedBuy.PlayWithDefaultAudiosource();
                    }
                }
                else if (FailedToBuySource)
                {
                    FailedToBuySource.Play();
                }
            }
        }
    }
}
