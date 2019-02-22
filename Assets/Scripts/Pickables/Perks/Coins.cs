using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : Pickable
{
    public int BonusScoreIncrease = 0;
    public int BonusCurrencyIncrease = 5;
    public GlobalEvents GlobalEvents;
    protected override bool OnPicked(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            GlobalEvents.AddBonusCurrency(BonusCurrencyIncrease);
            GlobalEvents.AddBonusScore(BonusScoreIncrease);
            return true;
        }
        return false;
    }
}
