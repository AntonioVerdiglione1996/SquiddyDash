using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Pickables/Mystery/Reward/Currency")]
public class MysteryBoxRewardCurrency : MysteryBoxRewardData
{
    public int Currency = 10;
    public int SkinParts = 2;
    public int AccessoryParts = 1;
    public float RarityAddMultiplier = 1f;
    protected override void OnValidate()
    {
        base.OnValidate();
        RarityAddMultiplier = Mathf.Max(RarityAddMultiplier, 0f);
        Currency = Mathf.Max(Currency, 0);
        SkinParts = Mathf.Max(SkinParts, 0);
        AccessoryParts = Mathf.Max(AccessoryParts, 0);
    }
    public override void Collect(MysteryBoxType type, CollectMysteryBoxesReward collector)
    {
        collector.Currency.ModifyGameCurrencyAmount(GetTotalValue(Currency, type), GetTotalValue(AccessoryParts, type), GetTotalValue(SkinParts, type));
    }
    private int GetTotalValue(int value, MysteryBoxType type)
    {
        return value + (int)(value * RarityAddMultiplier * (int)type);
    }
}
