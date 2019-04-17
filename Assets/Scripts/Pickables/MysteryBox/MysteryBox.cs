using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MysteryBox : Pickable
{
    public MysteryBoxType Type;
    public bool GenerateRandomTypeOnEnable = true;
    public MysteryBoxChances Chances;
    public GlobalEvents GlobalEvents;
    public GameObject[] Auras;
    private void OnEnable()
    {
        for (int i = 0; i < Auras.Length; i++)
        {
            GameObject itemaura = Auras[i];
            if (itemaura)
            {
                itemaura.SetActive(false);
            }
        }
        if (GenerateRandomTypeOnEnable && Chances)
        {
            MysteryBoxType ToSet = Chances.DefaultType;
            for (int i = 0; i < Chances.Data.Count; i++)
            {
                MysteryBoxChancesData Data = Chances.Data[i];
                if (UnityEngine.Random.Range(0f, 1f) <= Data.Chance)
                {
                    ToSet = Data.Type;
                    break;
                }
            }
            Type = ToSet;
        }
        GameObject aura = Auras[(int)Type];
        if (aura)
        {
            aura.SetActive(true);
        }
    }
    protected override void OnValidate()
    {
        base.OnValidate();
        int count = Enum.GetValues(typeof(MysteryBoxType)).Length;
        if (Auras.Length != count)
        {
            Auras = new GameObject[count];
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
