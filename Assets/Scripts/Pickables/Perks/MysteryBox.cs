using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : Pickable
{
    public MysteryBoxType Type;
    public GlobalEvents GlobalEvents;
    protected override bool OnPicked(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            GlobalEvents.AddCollectedBox(Type);
            return true;
        }
        return false;
    }
}
