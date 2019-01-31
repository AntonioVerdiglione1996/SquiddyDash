using UnityEngine;
using System;
public static class Spawner
{
    public static bool SpawnPrefabs(int count, SOPool pool, Transform parent = null, BasicEvent finishedSpawn = null, Action<GameObject, int> OnSpawn = null)
    {
        for (int i = 0; i < count; i++)
        {
            if (!SpawnPrefab(OnSpawn, i, pool, parent))
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
    private static bool SpawnPrefab(Action<GameObject, int> OnSpawn, int index, SOPool pool, Transform parent)
    {
        if (!pool)
        {
            return false;
        }
        int nullObj;
        bool parented;
        GameObject go = pool.DirectGet(parent, out nullObj, out parented);
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