using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
[System.Serializable]
public class MysteryTypeDescriber
{
    public Describer Describer;
    public MysteryBoxType Type;
    public MysteryTypeDescriber(Describer describer, MysteryBoxType type)
    {
        Describer = describer;
        Type = type;
    }
}
public class MysteryRewardShowcaser : MonoBehaviour
{
    public List<MysteryTypeDescriber> TypesDescribers = new List<MysteryTypeDescriber>();
    public Text TypeText;
    public TextMeshProUGUI TypeTextPro;
    public Text TypeDescriptionText;
    public TextMeshProUGUI TypeDescriptionTextPro;
    public Image TypeImage;
    public LerpedTextUI Currency;
    public LerpedTextUI Parts;
    public LerpedTextUI Skins;
    public Text AccessoriesText;
    public TextMeshProUGUI AccessoriesTextPro;
    public string AccessoryPrefix = "";
    public string AccessorySuffix = ".";
    public string AccessoriesSeparator = ", ";
    public void Showcase(MysteryBoxType type, List<IRewardCollected> rewards)
    {
        int totalCurrency = 0, totalAccParts = 0, totalSkins = 0;
        Utils.Builder.Clear();

        for (int i = 0; i < rewards.Count; i++)
        {
            IRewardCollected reward = rewards[i];
            if (reward != null && reward.IsRewardValid)
            {
                totalCurrency += reward.Currency;
                totalAccParts += reward.AccessoryParts;
                totalSkins += reward.SkinParts;
                if (reward.Unlocked)
                {
                    if (Utils.Builder.Length == 0)
                    {
                        Utils.Builder.Append(AccessoryPrefix);
                    }
                    else if (Utils.Builder.Length > 0)
                    {
                        Utils.Builder.Append(AccessoriesSeparator);
                    }
                    Utils.Builder.Append(reward.Unlocked.Describer.Name);
                }
            }
        }

        if (Utils.Builder.Length > 0)
        {
            Utils.Builder.Append(AccessorySuffix);
        }

        Describer typeDescriber = TypesDescribers.Find(x => x.Type == type).Describer;

        if(TypeText || TypeTextPro)
        {
            string text = typeDescriber.Name;
            if (TypeText)
            {
                TypeText.text = text;
            }
            if (TypeTextPro)
            {
                TypeTextPro.text = text;
            }
        }

        if (TypeDescriptionText || TypeDescriptionTextPro)
        {
            string text = typeDescriber.Description;
            if (TypeDescriptionText)
            {
                TypeDescriptionText.text = text;
            }
            if (TypeDescriptionTextPro)
            {
                TypeDescriptionTextPro.text = text;
            }
        }

        if (TypeImage)
        {
            TypeImage.sprite = typeDescriber.Image;
            TypeImage.material = typeDescriber.Material;
            TypeImage.color = typeDescriber.Color;
        }

        if (Currency)
        {
            Currency.StartValue = 0;
            Currency.EndValue = totalCurrency;
            Currency.RestartLerp();
        }
        if (Parts)
        {
            Parts.StartValue = 0;
            Parts.EndValue = totalAccParts;
            Parts.RestartLerp();
        }
        if (Skins)
        {
            Skins.StartValue = 0;
            Skins.EndValue = totalSkins;
            Skins.RestartLerp();
        }
        if (AccessoriesText || AccessoriesTextPro)
        {
            string text = Utils.Builder.ToString();
            if (AccessoriesText)
            {
                AccessoriesText.text = text;
            }
            if (AccessoriesTextPro)
            {
                AccessoriesTextPro.text = text;
            }
        }
        Utils.Builder.Clear();
    }
    protected virtual void OnValidate()
    {
        var types = System.Enum.GetValues(typeof(MysteryBoxType));
        if(TypesDescribers.Count != types.Length)
        {
            TypesDescribers = new List<MysteryTypeDescriber>(types.Length);
            foreach (MysteryBoxType item in types)
            {
                TypesDescribers.Add(new MysteryTypeDescriber(null, item));
            }
        }
        TypesDescribers.OrderBy(x => x.Type);
    }
}
