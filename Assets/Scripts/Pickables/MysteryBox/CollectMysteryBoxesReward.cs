using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMysteryBoxesReward : MonoBehaviour
{
    public GlobalEvents GlobalEvents;
    public StoringCurrentModelToSpawn Store;
    public MysteryBoxRewardContainer Container;
    public bool DebugEnabled = true;
    public Dictionary<MysteryBoxType, List<IRewardCollected>> Collected = new Dictionary<MysteryBoxType, List<IRewardCollected>>();
    public MysteryBoxRewardData DefaultReward;
    private void OnEnable()
    {
        var boxTypes = GlobalEvents.CollectedBoxes;
        if (boxTypes != null && boxTypes.Count > 0)
        {
            for (int i = 0; i < Store.Accessories.Count; i++)
            {
                Store.Accessories[i].Restore();
            }
            for (int i = boxTypes.Count - 1; i >= 0; i--)
            {
                bool collected = false;
                MysteryBoxType type = boxTypes[i];
                IRewardCollected rewardCollected = null;
                for (int j = 0; !collected && j < Container.Data.Length; j++)
                {
                    MysteryBoxRewardData Data = Container.Data[j];
                    if (Data)
                    {
                        for (int x = 0; x < Data.Types.Count; x++)
                        {
                            MysteryRewardChances Chances = Data.Types[x];
                            if (Chances.ValidChance && Chances.Type == type)
                            {
                                collected = UnityEngine.Random.Range(0f, 1f) <= Chances.Chance;
                                if (collected)
                                {
#if UNITY_EDITOR
                                    if (DebugEnabled)
                                    {
                                        Debug.LogFormat("{0} collected a reward of type {1} for a mystery box of type {2}", this, Data.GetType(), type);
                                    }
#endif
                                    rewardCollected = Data.Collect(Chances.Type, this);
                                }
                                break;
                            }
                        }
                    }
                }
                if (!collected)
                {
                    rewardCollected = DefaultReward.Collect(type, this);
                }
                boxTypes.RemoveAt(i);
                if (Collected.ContainsKey(type))
                {
                    Collected[type].Add(rewardCollected);
                }
                else
                {
                    var list = new List<IRewardCollected>();
                    list.Add(rewardCollected);
                    Collected.Add(type, list);
                }
            }
        }
        GlobalEvents.GameCurrency.SaveToFile();
        Store.VerifyAndSave();
        GlobalEvents.ClearCollectedBox();
    }
}
