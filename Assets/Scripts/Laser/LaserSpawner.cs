using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public PositionalEvent OnPlatformMoved;
    public Vector3 Offset = new Vector3(0f, 5.5f, 0f);
    public SOPool Pool;
    public Camera MainCamera;
    // Use this for initialization
    void OnEnable()
    {
        if(!MainCamera)
        {
            MainCamera = Camera.main;
        }
        Offset.x += MainCamera.transform.position.x - MainCamera.orthographicSize * MainCamera.aspect;
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
        if(Pool && ReferenceLocation)
        {
            int nullObj;
            Pool.Get(null, ReferenceLocation.position + Offset, out nullObj);
        }
    }
}
