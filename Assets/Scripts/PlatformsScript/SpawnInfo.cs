using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpawnInfo
{
    public SOPool Pool;
    public bool RandomSpawnLocation = false;
    public float SpawnChance;
    public int MinSpawnCount;
    public int MaxSpawnCount;
}
