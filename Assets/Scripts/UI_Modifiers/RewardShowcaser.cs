using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RewardShowcaser : MysteryRewardShowcaser
{
    public RectTransform MysteryBoxRewardShowcasersParent;
    public MysteryRewardShowcaser Prefab;
    public void OnRewardsCollected(MysteryBoxRewardCollection collection, IRewardCollected levelReward)
    {
        Showcase(MysteryBoxType.Common, new List<IRewardCollected>() { levelReward });
        foreach (var item in collection.Collected)
        {
            Instantiate(Prefab, MysteryBoxRewardShowcasersParent).Showcase(item.Key, item.Value);
        }
    }
}
