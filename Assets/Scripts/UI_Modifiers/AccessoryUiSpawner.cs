using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryUiSpawner : MonoBehaviour
{
    public AccessoryUICategory Prefab;
    public Transform UiParent;
    public Dictionary<EAccessoryType, AccessoryUICategory> Categories = new Dictionary<EAccessoryType, AccessoryUICategory>();
    void Start()
    {
        foreach (EAccessoryType item in System.Enum.GetValues(typeof(EAccessoryType)))
        {
            AccessoryUICategory cat = Instantiate(Prefab, UiParent);
            cat.SpawnType(item);
            Categories.Add(item, cat);
        }
    }
}
