using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryUiSpawner : MonoBehaviour
{
    public AccessoryUICategory Prefab;
    public Transform UiParent;
    public Dropdown Dropdown;
    public Dictionary<EAccessoryType, AccessoryUICategory> Categories = new Dictionary<EAccessoryType, AccessoryUICategory>();

    public EAccessoryType CurrentType { get; private set; }

    public PurchaseableUIUnlocker UnlockGO;
    void Start()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (EAccessoryType item in System.Enum.GetValues(typeof(EAccessoryType)))
        {
            AccessoryUICategory cat = Instantiate(Prefab, UiParent);
            cat.UnlockGO = UnlockGO;
            cat.SpawnType(item);
            cat.gameObject.SetActive(false);
            Categories.Add(item, cat);
            options.Add(new Dropdown.OptionData(item == EAccessoryType.None ? "Accessory":item.ToString()));
        }
        Dropdown.AddOptions(options);
        Dropdown.RefreshShownValue();
        Dropdown.onValueChanged.AddListener(OnValueChanged);
        OnValueChanged(Dropdown.value);
    }
    private void OnValueChanged(int newValue)
    {
        if (System.Enum.IsDefined(typeof(EAccessoryType), newValue))
        {
            EAccessoryType type = (EAccessoryType)newValue;
            if (Categories.ContainsKey(type))
            {
                if (Categories.ContainsKey(CurrentType))
                {
                    Categories[CurrentType].gameObject.SetActive(false);
                }
                Categories[type].gameObject.SetActive(true);
                CurrentType = type;
            }
        }
    }
    private void OnDestroy()
    {
        if (Dropdown)
        {
            Dropdown.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}
