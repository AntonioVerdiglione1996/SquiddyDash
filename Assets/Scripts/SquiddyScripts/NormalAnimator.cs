using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAnimator : SquiddyAnimator
{
    public BasicEvent OnLand;
    public BasicEvent OnJump;
    public string AnimJumpName = "IsJumping";
    protected virtual void OnEnable()
    {
        if (OnLand)
        {
            OnLand.OnEventRaised += UnsetJumpAnimation;
        }
        if (OnJump)
        {
            OnJump.OnEventRaised += SetJumpAnimation;
        }
    }
    protected virtual void OnDisable()
    {
        if (OnLand)
        {
            OnLand.OnEventRaised -= UnsetJumpAnimation;
        }
        if (OnJump)
        {
            OnJump.OnEventRaised -= SetJumpAnimation;
        }
    }
    public void SetJumpAnimation()
    {
        Anim.SetBool(AnimJumpName, true);
    }
    public void UnsetJumpAnimation()
    {
        Anim.SetBool(AnimJumpName, false);
    }
}
