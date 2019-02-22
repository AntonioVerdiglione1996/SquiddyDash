using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{

    public BasicEvent OnDynamicSpawnEvent;
    public BasicEvent OnGameoverEvent;

    public List<SpawnInfo> Info = new List<SpawnInfo>();

    public Camera MainCamera;

    public float HeightPadding = 5f;

    private bool spawn;
    private void OnValidate()
    {
        if (!MainCamera)
        {
            MainCamera = Camera.main;
        }
        if (Info == null)
        {
            Info = new List<SpawnInfo>();
        }
        for (int i = Info.Count - 1; i >= 0; i--)
        {
            SpawnInfo info = Info[i];
            if (info == null)
            {
                Info.RemoveAt(i);
            }
            else
            {
                info.SpawnChance = Mathf.Clamp01(info.SpawnChance);
                info.MinSpawnCount = Mathf.Max(info.MinSpawnCount, 0);
                info.MaxSpawnCount = Mathf.Max(info.MinSpawnCount, info.MinSpawnCount);
            }
        }
    }
    private void Awake()
    {
        EnableSpawn();
        OnGameoverEvent.OnEventRaised += DisableSpawn;
        OnDynamicSpawnEvent.OnEventRaised += DynamicSpawn;
    }
    public void DisableSpawn()
    {
        spawn = false;
    }
    public void EnableSpawn()
    {
        spawn = true;
    }
    public void DynamicSpawn()
    {
        if (!spawn)
        {
            return;
        }
        Bounds cameraBound = MainCamera.GetBounds();
        Bounds spawnBound = new Bounds(cameraBound.center + new Vector3(0f, cameraBound.max.y + HeightPadding, 0f), cameraBound.size);
        for (int i = 0; i < Info.Count; i++)
        {
            SpawnInfo info = Info[i];
            if (info != null && info.Pool && info.SpawnChance >= UnityEngine.Random.Range(0f, 1f))
            {
                int spawnCount = UnityEngine.Random.Range(info.MinSpawnCount, info.MaxSpawnCount + 1);
                for (int j = 0; j < spawnCount; j++)
                {
                    Vector3 spawnPos = spawnBound.GetRandomPoint();
                    spawnPos.z = 0f;
                    Spawner.SpawnPrefab(null, info.Pool, null, false, spawnPos);
                }
            }
        }
    }
    private void OnDestroy()
    {
        OnGameoverEvent.OnEventRaised -= DisableSpawn;
        OnDynamicSpawnEvent.OnEventRaised -= DynamicSpawn;
    }

}
