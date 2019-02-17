using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryUICategory : MonoBehaviour
{
    public AccessoryUI Prefab;
    public EAccessoryType Type;
    public StoringCurrentModelToSpawn scm;
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
                    AccessoryUI describer = Instantiate(Prefab, transform);
                    describer.SetDescriber(accessory.Describer);
                    describer.SetAccessory(i);
                }
            }
        }
    }
}
