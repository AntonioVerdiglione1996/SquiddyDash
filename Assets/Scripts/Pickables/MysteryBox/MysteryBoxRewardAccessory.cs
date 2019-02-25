using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Pickables/Mystery/Reward/Accessory")]
public class MysteryBoxRewardAccessory : MysteryBoxRewardData
{
    public EAccessoryRarity Rarity = EAccessoryRarity.Common;

    public int DefaultAccessoryParts = 50;

    public override void Collect(MysteryBoxType type, CollectMysteryBoxesReward collector)
    {
        StoringCurrentModelToSpawn store = collector.Store;
        bool accessoryUnlocked = false;

        //TODO: implement accessory reward

        if (!accessoryUnlocked)
        {
            collector.GlobalEvents.GameCurrency.ModifyGameCurrencyAmount(0, DefaultAccessoryParts, 0, false);
        }
    }
    protected override void OnValidate()
    {
        base.OnValidate();
        DefaultAccessoryParts = Mathf.Max(DefaultAccessoryParts, 0);
    }
}
