using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Gameplay/InfoSpawn")]
public class InfoSpawnContainer : ScriptableObject
{
    public SpawnInfo[] Info;
    public int Length { get { return Info.Length; } }
    public SpawnInfo this[int index]
    {
        get { return Info[index]; }
        set { Info[index] = value; }
    }
    void OnValidate()
    {
        if (Info == null)
        {
            Info = new SpawnInfo[0];
            return;
        }
        List<SpawnInfo> infos = new List<SpawnInfo>(Info);
        for (int i = infos.Count - 1; i >= 0; i--)
        {
            SpawnInfo info = infos[i];
            if (info == null)
            {
                infos.RemoveAt(i);
            }
            else
            {
                info.SpawnChance = Mathf.Clamp01(info.SpawnChance);
                info.MinSpawnCount = Mathf.Max(info.MinSpawnCount, 0);
                info.MaxSpawnCount = Mathf.Max(info.MinSpawnCount, info.MaxSpawnCount);
            }
        }
        Info = infos.OrderByDescending(x => x.SpawnChance).ToArray();
    }
}
