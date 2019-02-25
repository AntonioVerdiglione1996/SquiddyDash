using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMysteryBoxesReward : MonoBehaviour
{
    public GlobalEvents GlobalEvents;
    public InGameCurrency Currency;
    public StoringCurrentModelToSpawn Store;
    public MysteryBoxRewardContainer Container;
    public bool DebugEnabled = true;
    private void OnEnable()
    {
        var boxTypes = GlobalEvents.CollectedBoxes;
        if (boxTypes != null)
        {
            for (int i = boxTypes.Count - 1; i >= 0; i--)
            {
                bool collected = false;
                MysteryBoxType type = boxTypes[i];
                for (int j = 0; !collected && j < Container.Data.Length; j++)
                {
                    MysteryBoxRewardData Data = Container.Data[j];
                    if (Data)
                    {
                        for (int x = 0; x < Data.Types.Count; x++)
                        {
                            MysteryRewardChances Chances = Data.Types[x];
                            if (Chances.Type == type)
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
                                    Data.Collect(Chances.Type, this);
                                }
                                break;
                            }
                        }
                    }
                }
                boxTypes.RemoveAt(i);
            }
        }
        GlobalEvents.ClearCollectedBox();
    }
}
