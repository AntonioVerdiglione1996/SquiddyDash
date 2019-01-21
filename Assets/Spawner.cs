using UnityEngine;
using System;
public static class Spawner
{
    public static bool SpawnPrefabs(int count, GameObject prefab, Transform parent = null, BasicEvent finishedSpawn = null, Action<GameObject, int> OnSpawn = null)
    {
        for (int i = 0; i < count; i++)
        {
            if (!SpawnPrefab(OnSpawn, i, prefab, parent))
            {
                return false;
            }
        }
        if (finishedSpawn)
        {
            finishedSpawn.Raise();
        }
        return true;
    }
    private static bool SpawnPrefab(Action<GameObject, int> OnSpawn, int index, GameObject prefab, Transform parent)
    {
        if (!prefab)
        {
            return false;
        }
        GameObject go = GameObject.Instantiate(prefab, parent);
        if (!go)
        {
            return false;
        }
        if (OnSpawn != null)
        {
            OnSpawn(go, index);
        }
        return true;
    }
}