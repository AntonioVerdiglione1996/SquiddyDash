using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillImprover : Pickable
{
    public float ImproveAmount = 1.2f;
    protected override bool OnPicked(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            SquiddyController character = other.transform.root.GetComponentInChildren<SquiddyController>();
            if (character && character.OwnedCharacter && character.OwnedCharacter.MainSkill)
            {
                character.OwnedCharacter.MainSkill.ImproveInvokability(ImproveAmount);
            }
            return true;
        }
        return false;
    }
}
