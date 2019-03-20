using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryUICategory : MonoBehaviour
{
    public Transform UiParent;
    public EAccessoryType Type;
    public StoringCurrentModelToSpawn scm;

    public PurchaseableUI Prefab;
    public PurchaseableUIUnlocker UnlockGO;
    private void AddAccessory(int index, IPurchaseObject obj)
    {
        scm.AddAccessory(index);
    }
    public void SpawnType(EAccessoryType type)
    {
        Type = type;

        if (scm.Accessories != null)
        {
            for (int i = 0; i < scm.Accessories.Count; i++)
            {
                Accessory accessory = scm.Accessories[i];
                if (accessory && accessory.Type == Type)
                {
                    accessory.PurchaseInfo.RestoreFromFile(true);
                    PurchaseableUI p = Instantiate(Prefab, UiParent);
                    p.OnClickObjPurchasedEvent.AddListener(AddAccessory);
                    p.SetPurchaseable(accessory, UnlockGO, i);
                }
            }
        }
    }
}
