using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryUICategory : MonoBehaviour
{
    public AccessoryUI Prefab;
    public Transform UiParent;
    public EAccessoryType Type;
    public StoringCurrentModelToSpawn scm;
    public AccessoryUnlocker UnlockGO;

    public PurchaseableUI ActualPrefab;
    public PurchaseableUIUnlocker ActualUnlockGO;
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
                    if (!ActualPrefab || !ActualUnlockGO)
                    {
                        AccessoryUI describer = Instantiate(Prefab, UiParent);
                        describer.UnlockGO = UnlockGO;
                        describer.SetDescriber(accessory.Describer);
                        describer.SetAccessory(accessory, i);
                    }
                    else
                    {
                        PurchaseableUI p = Instantiate(ActualPrefab, UiParent);
                        p.OnClickObjPurchasedEvent.AddListener(AddAccessory);
                        p.SetPurchaseable(accessory, ActualUnlockGO, i);
                    }
                }
            }
        }
    }
}
