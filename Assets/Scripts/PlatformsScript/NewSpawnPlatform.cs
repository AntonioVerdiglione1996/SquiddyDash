﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpawnPlatform : MonoBehaviour
{
    public Transform Squiddy;
    public SOPool PlatformPool;
    public SOPool PlatformWithPowerUpPool;
    public Material Material;

    public PositionalEvent OnPlatformSpawn;

    public uint NumberOfPlatformsToSpawn = 4;

    public Vector3 VectorToAdd = new Vector3(0f, 11f, 0f);
    public int treshHold = 25;

    public float CurrentSpeedMultiplier = 1f;
    public Vector3 CurrentScaleMultiplier = Vector3.one;

    public uint SpawnedPlatformCount { get; private set; }
    public uint SpecialPlatformSpawnIntervall = 9;

    public BasicEvent OnPlatformRecycle;

    private Vector3 newPos;

    private void Awake()
    {
        SpawnedPlatformCount = 1;
        newPos = VectorToAdd;
        if(OnPlatformRecycle)
        {
            OnPlatformRecycle.OnEventRaised += SpawnPlatforms;
        }

        SpawnPlatforms(NumberOfPlatformsToSpawn, true);

    }
    private void OnDestroy()
    {
        if (OnPlatformRecycle)
        {
            OnPlatformRecycle.OnEventRaised -= SpawnPlatforms;
        }
    }
    public void SpawnPlatforms()
    {
        SpawnPlatforms(1, false);
    }
    public void SpawnPlatforms(uint count, bool setMaterial = false)
    {
        for (int i = 0; i < count; i++)
        {
            //normal platform spawn
            if (SpawnedPlatformCount > 0 && (SpawnedPlatformCount % SpecialPlatformSpawnIntervall == 0))
            {
                SpawnPlatform(PlatformWithPowerUpPool, setMaterial);
            }
            //powerUp
            else
            {
                SpawnPlatform(PlatformPool, setMaterial);
            }
        }
    }
    private GameObject SpawnPlatform(SOPool pool, bool setMaterials = false)
    {
        int nullObj;
        GameObject go = pool.Get(null, newPos, out nullObj, true);

        Transform root = go.transform.root;
        root.localScale = CurrentScaleMultiplier;

        NewMovePlatform mover = go.GetComponentInChildren<NewMovePlatform>();
        if(mover)
        {
            mover.Speed = mover.InitialSpeed * CurrentSpeedMultiplier;
        }

        if (setMaterials && Material != null)
        {
            Platform plat = go.GetComponentInChildren<Platform>();
            if (plat)
            {
                plat.SetMaterial(Material);
            }
        }

        newPos += VectorToAdd;
        SpawnedPlatformCount++;

        if (OnPlatformSpawn)
        {
            OnPlatformSpawn.Location = go.transform;
            OnPlatformSpawn.Raise();
        }

        return go;
    }
}
