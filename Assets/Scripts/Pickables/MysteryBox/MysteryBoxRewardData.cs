using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class MysteryBoxRewardData : ScriptableObject
{
    public List<MysteryRewardChances> Types;
    public abstract void Collect(MysteryBoxType type, CollectMysteryBoxesReward collector);
    protected virtual void OnValidate()
    {
        var values = System.Enum.GetValues(typeof(MysteryBoxType));
        if (Types == null || Types.Count == 0)
        {
            Types = new List<MysteryRewardChances>(values.Length);
            foreach (MysteryBoxType item in values)
            {
                Types.Add(new MysteryRewardChances(item, true));
            }
            return;
        }
        foreach (MysteryBoxType item in values)
        {
            if (Types.All(x => x.Type != item))
            {
                Types.Add(new MysteryRewardChances(item, true));
            }
        }
        for (int i = Types.Count - 1; i >= 0; i--)
        {
            MysteryRewardChances data = Types[i];
            data.Chance = Mathf.Clamp01(data.Chance);
            data.ValidChance = !Mathf.Approximately(data.Chance, 0f);
            Types[i] = data;
            if (Types.Count(x => x.Type == data.Type) > 1)
            {
                Types.RemoveAt(i);
            }
        }
        Types.OrderByDescending(x => x.Chance);
    }
}
