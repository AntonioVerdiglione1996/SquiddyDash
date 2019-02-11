﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Accessory : MonoBehaviour
{
    public Describer Describer;
    public Transform Root;
    public Skill Skill;
    public List<Upgrade> Upgrades = new List<Upgrade>();
    public EAccessoryType Type;
    public EAccessoryRarity Rarity
    {
        get
        {
            EAccessoryRarity type = EAccessoryRarity.Common;
            if (Skill)
            {
                if (Skill is PassiveSkill)
                {
                    type |= EAccessoryRarity.Rare;
                }
                else
                {
                    type |= EAccessoryRarity.Legendary;
                }
            }
            if (Upgrades != null && Upgrades.Count > 0)
            {
                type |= EAccessoryRarity.Special;
            }
            return type;
        }
    }
    public void SetParent(Transform parent, bool worldPositionStays = false)
    {
        OnValidate();
        Root.SetParent(parent, worldPositionStays);
    }
    public void SetParent(Transform parent, Vector3 localPosition)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Vector3 localScale)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localScale = localScale;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Quaternion localRotation)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localRotation = localRotation;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localRotation = localRotation;
        Root.localScale = localScale;
    }
    private void Awake()
    {
        OnValidate();
        if (Upgrades != null)
        {
            for (int i = Upgrades.Count - 1; i > 0; i--)
            {
                if (!Upgrades[i])
                {
                    Upgrades.RemoveAt(i);
                }
            }
        }
    }
    private void OnValidate()
    {
        if (!Skill)
        {
            Skill = GetComponent<Skill>();
        }
        if (!Root)
        {
            Root = transform;
        }
    }
}
