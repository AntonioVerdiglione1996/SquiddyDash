using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveCurrentAccessory : MonoBehaviour
{
    public StoringCurrentModelToSpawn Scm;
    public AccessoryUiSpawner Spawner;
    public void RemoveAccessory()
    {
        Scm.RemoveAccessory(Spawner.CurrentType);
    }
}
