using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public PositionalEvent OnPlatformMoved;
    public Vector3 Offset = new Vector3(0f, 5.5f, 0f);
    public SOPool Pool;
    public Camera MainCamera;
    public float SpawnChance = 0.1f;
    // Use this for initialization
    void OnEnable()
    {
        if (!MainCamera)
        {
            MainCamera = Camera.main;
            if (MainCamera)
            {
                Offset.x += MainCamera.transform.position.x - MainCamera.orthographicSize * MainCamera.aspect;
            }
        }
        if (OnPlatformMoved)
        {
            OnPlatformMoved.OnPositionalRaised += SpawnLaser;
        }
    }
    private void OnDisable()
    {
        if (OnPlatformMoved)
        {
            OnPlatformMoved.OnPositionalRaised -= SpawnLaser;
        }
    }
    public void SpawnLaser(Transform ReferenceLocation)
    {
        if (Pool && ReferenceLocation)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= SpawnChance)
            {
                int nullObj;
                Pool.Get(null, ReferenceLocation.position + Offset, out nullObj);
            }
        }
    }
}
