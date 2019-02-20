using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpawnPlatform : MonoBehaviour
{
    public BasicEvent OnGameOver;
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

    public LinkedList<Platform> ActivePlatforms = new LinkedList<Platform>();

    private Vector3 newPos;

    private bool spawn = true;
    private void OnEnable()
    {
        SpawnedPlatformCount = 1;
        newPos = VectorToAdd;
        if (OnPlatformRecycle)
        {
            OnPlatformRecycle.OnEventRaised += SpawnPlatforms;
        }
        if (OnGameOver)
        {
            OnGameOver.OnEventRaised += DisableSpawn;
        }
        SpawnPlatforms(NumberOfPlatformsToSpawn, true);

    }
    public void EnableSpawn()
    {
        spawn = true;
    }
    public void DisableSpawn()
    {
        spawn = false;
    }
    private void OnDisable()
    {
        if (OnPlatformRecycle)
        {
            OnPlatformRecycle.OnEventRaised -= SpawnPlatforms;
        }
        if (OnGameOver)
        {
            OnGameOver.OnEventRaised -= DisableSpawn;
        }
    }
    public void SpawnPlatforms()
    {
        SpawnPlatforms(1, false);
    }
    public void SpawnPlatforms(uint count, bool setMaterial = false)
    {
        if (!spawn)
        {
            return;
        }
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

        Platform plat = go.GetComponentInChildren<Platform>();
        NewMovePlatform mover = go.GetComponentInChildren<NewMovePlatform>();
        if (plat)
        {
            plat.Node = ActivePlatforms.AddLast(plat);
            plat.transform.localScale = CurrentScaleMultiplier;
            if (setMaterials && Material != null)
            {
                plat.SetMaterial(Material);
            }
            if (plat.PlatCollider && mover)
            {
                mover.CollisionBounds = new Bounds(plat.transform.root.position, plat.PlatCollider.transform.lossyScale);
            }
        }

        if (mover)
        {
            mover.Speed = mover.InitialSpeed * CurrentSpeedMultiplier;
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
