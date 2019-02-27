using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Pickables/Mystery/Reward/Container")]
public class MysteryBoxRewardContainer : ScriptableObject
{
    public MysteryBoxRewardData[] Data = new MysteryBoxRewardData[0];
}
