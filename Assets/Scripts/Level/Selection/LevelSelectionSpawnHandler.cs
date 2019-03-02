using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionSpawnHandler : MonoBehaviour
{
    public ReusableLevelSelection LevelSelection;
    public Transform ContainerParentTransform;
    public BasicSOPool LevelUIPrefab;
    public LevelContainer Container;
    void Start()
    {
        if (Spawner.SpawnPrefabs(Container.Datas.Length, LevelUIPrefab, ContainerParentTransform, null, OnGOSpawned))
        {
            LevelSelection.Initialize();
        }
    }
    private void OnGOSpawned(GameObject spawned, int index)
    {
        LevelUIHandler ui = spawned.GetComponentInChildren<LevelUIHandler>(true);
        if (ui)
        {
            ui.Initialize(Container.Datas[index]);
        }
    }
}
