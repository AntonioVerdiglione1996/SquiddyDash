using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Gameplay/MysteryBoxChances")]
public class MysteryBoxChances : ScriptableObject
{
    public List<MysteryBoxChancesData> Data;
    public MysteryBoxType DefaultType = MysteryBoxType.Normal;
    private void OnValidate()
    {
        var values = System.Enum.GetValues(typeof(MysteryBoxType));
        if (Data == null || Data.Count == 0)
        {
            Data = new List<MysteryBoxChancesData>(values.Length);
            foreach (MysteryBoxType item in values)
            {
                Data.Add(new MysteryBoxChancesData(item));
            }
            return;
        }
        foreach (MysteryBoxType item in values)
        {
            if (Data.All(x => x.Type != item))
            {
                Data.Add(new MysteryBoxChancesData(item));
            }
        }
        for (int i = Data.Count - 1; i >= 0; i--)
        {
            MysteryBoxChancesData data = Data[i];
            data.Chance = Mathf.Clamp01(data.Chance);
            if(Data.Count(x => x.Type == data.Type) > 1)
            {
                Data.RemoveAt(i);
            }
        }
        Data.OrderByDescending(x => x.Chance);
    }
}
