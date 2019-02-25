using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : Pickable
{
    public MysteryBoxType Type;
    public bool GenerateRandomTypeOnEnable = true;
    public MysteryBoxChances Chances;
    public GlobalEvents GlobalEvents;
    private void OnEnable()
    {
        if (GenerateRandomTypeOnEnable && Chances)
        {
            for (int i = 0; i < Chances.Data.Count; i++)
            {
                MysteryBoxChancesData Data = Chances.Data[i];
                if (UnityEngine.Random.Range(0f, 1f) <= Data.Chance)
                {
                    Type = Data.Type;
                    return;
                }
            }
            Type = Chances.DefaultType;
        }
    }
    protected override bool OnPicked(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            GlobalEvents.AddCollectedBox(Type);
            return true;
        }
        return false;
    }
}
