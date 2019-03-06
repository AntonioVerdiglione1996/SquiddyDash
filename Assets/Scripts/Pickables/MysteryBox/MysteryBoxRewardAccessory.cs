using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Gameplay/Pickables/Mystery/Reward/Accessory")]
public class MysteryBoxRewardAccessory : MysteryBoxRewardData
{
    public EAccessoryRarity Rarity = EAccessoryRarity.Common;

    public int DefaultAccessoryParts = 50;

    private List<Accessory> accessoriesToChose;

    private Accessory lastUnlocked;

    public override IRewardCollected Collect(MysteryBoxType type, CollectMysteryBoxesReward collector)
    {
        if (!UnlockAccessory(type, collector))
        {
#if UNITY_EDITOR
            if (DebugEnabled)
            {
                Debug.LogFormat("{0} could not find a valid accessory of rarity {1} to unlock, default reward {2}", this, Rarity, collector.GlobalEvents.GameCurrency.CanModifyGameCurrency(0, DefaultAccessoryParts, 0) ? "given" : "not given");
            }
#endif

            if (collector.GlobalEvents.GameCurrency.ModifyGameCurrencyAmount(0, DefaultAccessoryParts, 0, false))
            {
                return new RewardCollectedInfo(0, DefaultAccessoryParts, 0, null);
            }
            else
            {
                return new RewardCollectedInfo(0, 0, 0, null);
            }
        }
        return new RewardCollectedInfo(0, 0, 0, lastUnlocked);
    }
    protected override void OnValidate()
    {
        base.OnValidate();
        DefaultAccessoryParts = Mathf.Max(DefaultAccessoryParts, 0);
    }
    protected virtual bool UnlockAccessory(MysteryBoxType type, CollectMysteryBoxesReward collector)
    {
        StoringCurrentModelToSpawn store = collector.Store;
        if (accessoriesToChose == null)
        {
            accessoriesToChose = new List<Accessory>();
        }
        accessoriesToChose.Clear();

        for (int i = 0; i < store.Accessories.Count; i++)
        {
            Accessory accessory = store.Accessories[i];
            if (accessory && accessory.Rarity == Rarity && !accessory.IsUnlocked)
            {
                accessoriesToChose.Add(accessory);
            }
        }

        if (accessoriesToChose.Count <= 0)
        {
            return false;
        }

        int indexToUnlock = UnityEngine.Random.Range(0, accessoriesToChose.Count);

        lastUnlocked = accessoriesToChose[indexToUnlock];
        lastUnlocked.IsUnlocked = true;
#if UNITY_EDITOR
        if (DebugEnabled)
        {
            Debug.LogFormat("{0} unlocked the accessory {1} of rarity {2}", this, lastUnlocked, Rarity);
        }
#endif

        return true;
    }
}
