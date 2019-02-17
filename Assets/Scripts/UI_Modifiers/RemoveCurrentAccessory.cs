using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveCurrentAccessory : MonoBehaviour
{
    public StoringCurrentModelToSpawn Scm;
    public AccessoryUiSpawner Spawner;
    public Character_Manager Manager;
    private void OnValidate()
    {
        Awake();
    }
    private void Awake()
    {
        if (!Spawner)
        {
            Spawner = FindObjectOfType<AccessoryUiSpawner>();
        }
        if (!Manager)
        {
            Manager = FindObjectOfType<Character_Manager>();
        }
    }
    public void RemoveAccessory()
    {
        Scm.RemoveAccessory(Spawner.CurrentType);
        if (Manager.GetCurrentModel)
        {
            Manager.GetCurrentModel.CollectAndSpawnSkills(Scm.Accessories, Scm.GetAccessoriesIndices(), false);
        }
    }
}
