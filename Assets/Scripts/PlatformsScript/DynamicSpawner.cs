using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{

    public BasicEvent OnDynamicSpawnEvent;
    public BasicEvent OnGameoverEvent;

    private bool spawn;
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
    }
    private void OnDestroy()
    {
        OnGameoverEvent.OnEventRaised -= DisableSpawn;
        OnDynamicSpawnEvent.OnEventRaised -= DynamicSpawn;
    }

}
