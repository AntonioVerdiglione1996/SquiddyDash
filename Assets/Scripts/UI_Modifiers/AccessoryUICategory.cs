using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryUICategory : MonoBehaviour {
    public EAccessoryType Type;
    public StoringCurrentModelToSpawn scm;
    public void SpawnType(EAccessoryType type)
    {
        Type = type;
        if(scm.Accessories != null)
        {
            for (int i = 0; i < scm.Accessories.Count; i++)
            {
                Accessory accessory = scm.Accessories[i];
                if(accessory && accessory.Type == Type)
                {
                    Instantiate(accessory, transform);
                }
            }
        }
    }
    //Spawna tutto in child
}
