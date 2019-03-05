using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public BasicEvent OnGameOver;
    public PositionalEvent OnPlatformMoved;
    public Vector3 Offset = new Vector3(0f, 5.5f, 0f);
    public BasicSOPool LaserPrefab;
    public Camera MainCamera;
    public float SpawnChance = 0.2f;
    public float ScoreMultiplierAddedToSpawnChance = 0.001f;
    public bool DebugDisableSpawn = true;
    public ScoreSystem Score;
    public float LeftOffset = -3f;


    private bool spawn = true;
    // Use this for initialization
    void OnEnable()
    {
        spawn = true;
        if (!MainCamera)
        {
            MainCamera = Camera.main;
        }
        if (MainCamera)
        {
            Bounds cameraBound = MainCamera.GetBounds();
            Offset.x += cameraBound.center.x - cameraBound.extents.x + LeftOffset;
        }
        if (OnPlatformMoved)
        {
            OnPlatformMoved.OnPositionalRaised += SpawnLaser;
        }
        if (OnGameOver)
        {
            OnGameOver.OnEventRaised += DisableSpawn;
        }
    }
    public void DisableSpawn()
    {
        spawn = false;
    }
    public void EnableSpawn()
    {
        spawn = true;
    }
    private void OnDisable()
    {
        if (OnPlatformMoved)
        {
            OnPlatformMoved.OnPositionalRaised -= SpawnLaser;
        }
        if (OnGameOver)
        {
            OnGameOver.OnEventRaised -= DisableSpawn;
        }
    }
    public void SpawnLaser(Transform ReferenceLocation)
    {
        if (!spawn)
        {
            return;
        }
#if UNITY_EDITOR
        if (DebugDisableSpawn)
        {
            return;
        }
#endif
        if (LaserPrefab != null && ReferenceLocation)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= SpawnChance + ScoreMultiplierAddedToSpawnChance * Score.Score)
            {
                Spawner.SpawnPrefab(null, LaserPrefab, null, false, ReferenceLocation.position + Offset);
            }
        }
    }
}
