using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IRewardCollected
{
    int Currency { get; set; }
    int AccessoryParts { get; set; }
    int SkinParts { get; set; }
    Accessory Unlocked { get; set; }
    bool IsRewardValid { get; }
}
[System.Serializable]
public struct RewardCollectedInfo : IRewardCollected
{
    public int Currency { get; set; }
    public int AccessoryParts { get; set; }
    public int SkinParts { get; set; }
    public Accessory Unlocked { get; set; }

    public bool IsRewardValid
    {
        get
        {
            return Currency != 0 || AccessoryParts != 0 || SkinParts != 0 || Unlocked;
        }
    }

    public RewardCollectedInfo(int currency, int accessoryParts, int skinParts, Accessory unlocked)
    {
        Currency = currency;
        AccessoryParts = accessoryParts;
        SkinParts = skinParts;
        Unlocked = unlocked;
    }
}
public abstract class MysteryBoxRewardData : ScriptableObject
{
    public List<MysteryRewardChances> Types;
    public bool DebugEnabled = true;
    public abstract IRewardCollected Collect(MysteryBoxType type, CollectMysteryBoxesReward collector);
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
