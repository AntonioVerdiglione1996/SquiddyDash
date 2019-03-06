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
    public override IRewardCollected Collect(MysteryBoxType type, CollectMysteryBoxesReward collector)
    {
#if UNITY_EDITOR
        if (DebugEnabled)
        {
            Debug.LogFormat("{0} {1} to increase currency by {2}, {3}, {4}", this, collector.GlobalEvents.GameCurrency.CanModifyGameCurrency(GetTotalValue(Currency, type), GetTotalValue(AccessoryParts, type), GetTotalValue(SkinParts, type)) ? "managed" : "could not manage", GetTotalValue(Currency, type), GetTotalValue(AccessoryParts, type), GetTotalValue(SkinParts, type));
        }
#endif
        if (collector.GlobalEvents.GameCurrency.ModifyGameCurrencyAmount(GetTotalValue(Currency, type), GetTotalValue(AccessoryParts, type), GetTotalValue(SkinParts, type), false))
        {
            return new RewardCollectedInfo(GetTotalValue(Currency, type), GetTotalValue(AccessoryParts, type), GetTotalValue(SkinParts, type), null);
        }
        else
        {
            return new RewardCollectedInfo(0, 0, 0, null);
        }
    }
    private int GetTotalValue(int value, MysteryBoxType type)
    {
        return value + (int)(value * RarityAddMultiplier * (int)type);
    }
}
