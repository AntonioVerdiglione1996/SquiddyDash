using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionSpawnHandler : MonoBehaviour
{
    public ReusableLevelSelection LevelSelection;
    public Spawner Spawner;
    public LevelContainer Container;
    void Start()
    {
        if(Spawner.SpawnPrefabs(Container.Datas.Length, OnGOSpawned))
        {
            LevelSelection.Initialize();
        }
    }
    private void OnGOSpawned(GameObject spawned, int index)
    {
        LevelUIHandler ui = spawned.GetComponentInChildren<LevelUIHandler>(true);
        if(ui)
        {
            ui.LevelData = Container.Datas[index];
            ui.Initialize();
        }
    }
}
