using UnityEngine;
using System;
public class Spawner : MonoBehaviour
{
    public Transform Parent;
    public GameObject Prefab;
    public GameEvent FinishedSpawn;
    public bool SpawnPrefabs(int count, Action<GameObject, int> OnSpawn = null)
    {
        for (int i = 0; i < count; i++)
        {
            if (!SpawnPrefab(OnSpawn, i))
            {
                return false;
            }
        }
        if (FinishedSpawn)
        {
            FinishedSpawn.Raise();
        }
        return true;
    }
    private bool SpawnPrefab(Action<GameObject, int> OnSpawn, int index)
    {
        if (!Prefab)
        {
            return false;
        }
        GameObject go = GameObject.Instantiate(Prefab, Parent);
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