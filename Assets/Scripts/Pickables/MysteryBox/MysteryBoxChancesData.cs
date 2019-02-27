using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MysteryBoxChancesData
{
    public MysteryBoxType Type;
    public float Chance;
    public MysteryBoxChancesData()
    {
        Type = MysteryBoxType.Common;
        Chance = 0f;
    }
    public MysteryBoxChancesData(MysteryBoxType Type, float Chance = 1f)
    {
        this.Type = Type;
        this.Chance = Chance;
    }
}
