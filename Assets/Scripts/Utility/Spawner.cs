using UnityEngine;
using System;
public static class Spawner
{
    public static bool SpawnPrefabs(int count, BasicSOPool pool, Transform parent = null, BasicEvent finishedSpawn = null, Action<GameObject, int> OnSpawn = null, bool directGet = false)
    {
        for (int i = 0; i < count; i++)
        {
            if (!SpawnPrefab(OnSpawn, i, pool, parent, directGet))
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
    public static GameObject SpawnPrefab(Action<GameObject> OnSpawn, BasicSOPool pool, Transform parent, bool directGet, Vector3 position, Quaternion rotation)
    {
        GameObject go = SpawnPrefab(OnSpawn, pool, parent, directGet);
        if (go)
        {
            go.transform.SetPositionAndRotation(position, rotation);
        }
        return go;
    }
    public static GameObject SpawnPrefab(Action<GameObject> OnSpawn, BasicSOPool pool, Transform parent, bool directGet, Vector3 position)
    {
        GameObject go = SpawnPrefab(OnSpawn, pool, parent, directGet);
        if (go)
        {
            go.transform.position = position;
        }
        return go;
    }
    public static GameObject SpawnPrefab(Action<GameObject> OnSpawn, BasicSOPool pool, Transform parent, bool directGet)
    {
        GameObject obj = SpawnPrefab(null, 0, pool, parent, directGet);
        if (obj && OnSpawn != null)
        {
            OnSpawn(obj);
        }
        return obj;
    }

    private static GameObject SpawnPrefab(Action<GameObject, int> OnSpawn, int index, BasicSOPool pool, Transform parent, bool directGet)
    {
        if (pool == null)
        {
            return null;
        }
        int nullObj;
        bool parented;
        GameObject go = directGet ? pool.DirectGet(parent, out nullObj, out parented) : pool.Get(parent, out nullObj, true);
        if (!go)
        {
            return null;
        }
        ISOPoolable poolable = go.GetComponent<ISOPoolable>();
        if (!poolable)
        {
            poolable = go.GetComponentInChildren<ISOPoolable>(true);
        }
        if (poolable)
        {
            poolable.Pool = pool;
            if (!poolable.Root)
            {
                poolable.Root = poolable.gameObject;
            }
        }
        if (OnSpawn != null)
        {
            OnSpawn(go, index);
        }
        return go;
    }
}