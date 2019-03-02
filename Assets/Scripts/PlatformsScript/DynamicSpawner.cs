using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{

    public BasicEvent OnDynamicSpawnEvent;
    public BasicEvent OnGameoverEvent;

    public InfoSpawnContainer Info;

    public Camera MainCamera;

    public Vector3 SizePadding = new Vector3(1f, 2f, 0f);

    public bool UseGrid = true;

    public int GridWidthSize = 30;
    public int GridHeightSize = 30;

    private int[] gridIndices;
    private int currentGridIndicesIndex = 0;
    private bool spawn;

    public void DisableSpawn()
    {
        spawn = false;
    }
    public void EnableSpawn()
    {
        spawn = true;
    }
    public float GetInverseGridWidth()
    {
        return 1f / GetGridWidth();
    }
    public float GetInverseGridHeight()
    {
        return 1f / GetGridHeight();
    }
    public float GetInverseGridSize()
    {
        return 1f / GetGridSize();
    }
    public int GetGridWidth()
    {
        return Mathf.Max(GridWidthSize, 1);
    }
    public int GetGridHeight()
    {
        return Mathf.Max(GridHeightSize, 1);
    }
    public int GetGridSize()
    {
        return GetGridHeight() * GetGridWidth();
    }
    public void DynamicSpawn()
    {
        if (!spawn || !Info)
        {
            return;
        }
        if (UseGrid)
        {
            UpdateGrid();
        }
        Bounds cameraBound = MainCamera.GetBounds();
        Bounds spawnBound = new Bounds(cameraBound.center + new Vector3(0f, cameraBound.size.y, 0f), cameraBound.size - SizePadding);
        for (int i = 0; i < Info.Length; i++)
        {
            SpawnInfo info = Info[i];
            if (!info.RandomSpawnLocation && UseGrid)
            {
                if (!GridSpawn(info, spawnBound))
                {
                    break;
                }
            }
            else
            {
                NormalSpawn(info, spawnBound);
            }
        }
    }

    private void UpdateGrid()
    {
        if (gridIndices == null || gridIndices.Length != GetGridSize())
        {
            gridIndices = new int[GetGridSize()];
            for (int i = 0; i < gridIndices.Length; i++)
            {
                gridIndices[i] = i;
            }
        }
        gridIndices.Shuffle();
        currentGridIndicesIndex = 0;
    }
    private Vector3 GetGridPosition(Bounds spawnBound, int index)
    {
        Vector2Int bidimensionalIndex = Utils.GetBidimensionalIndex(index, GetGridWidth());
        if (index >= GetGridSize())
        {
#if UNITY_EDITOR
            Debug.LogException(new System.IndexOutOfRangeException(this + " could not find a valid position in the spawn grid, the requested index is out of the grid bounds. Index :" + index + ", size:" + GetGridSize()));
#endif
            return spawnBound.center;
        }
        float widthStep = spawnBound.size.x * GetInverseGridWidth();
        float heightStep = spawnBound.size.y * GetInverseGridHeight();
        return spawnBound.min + new Vector3(bidimensionalIndex.x * widthStep, bidimensionalIndex.y * heightStep, 0f);
    }
    private bool GridSpawn(SpawnInfo info, Bounds spawnBound)
    {
        if (gridIndices.Length <= currentGridIndicesIndex)
        {
            return false;
        }
        if (IsSpawnValid(info))
        {
            int spawnCount = UnityEngine.Random.Range(info.MinSpawnCount, info.MaxSpawnCount + 1);
            for (int j = 0; j < spawnCount; j++)
            {
                if (gridIndices.Length <= currentGridIndicesIndex)
                {
                    return false;
                }
                Spawn(info, GetGridPosition(spawnBound, gridIndices[currentGridIndicesIndex]));
                currentGridIndicesIndex++;
            }
        }
        return true;
    }
    private void NormalSpawn(SpawnInfo info, Bounds spawnBound)
    {
        if (IsSpawnValid(info))
        {
            int spawnCount = UnityEngine.Random.Range(info.MinSpawnCount, info.MaxSpawnCount + 1);
            for (int j = 0; j < spawnCount; j++)
            {
                Spawn(info, spawnBound.GetRandomPoint());
            }
        }
    }
    private bool IsSpawnValid(SpawnInfo info)
    {
        return info != null && info.ObjPrefab != null && info.SpawnChance >= UnityEngine.Random.Range(0f, 1f);
    }
    private void Spawn(SpawnInfo info, Vector3 position)
    {
        position.z = 0f;
        Spawner.SpawnPrefab(null, info.ObjPrefab, null, false, position);
    }

    private void OnValidate()
    {
        GridWidthSize = GetGridWidth();
        GridHeightSize = GetGridHeight();
        if (!MainCamera)
        {
            MainCamera = Camera.main;
        }
    }
    private void Awake()
    {
        EnableSpawn();
        OnGameoverEvent.OnEventRaised += DisableSpawn;
        if (OnDynamicSpawnEvent)
        {
            OnDynamicSpawnEvent.OnEventRaised += DynamicSpawn;
        }
    }
    private void OnDestroy()
    {
        OnGameoverEvent.OnEventRaised -= DisableSpawn;
        if (OnDynamicSpawnEvent)
        {
            OnDynamicSpawnEvent.OnEventRaised -= DynamicSpawn;
        }
    }

}
