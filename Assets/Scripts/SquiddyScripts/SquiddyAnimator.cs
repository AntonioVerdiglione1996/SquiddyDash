using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public abstract class SquiddyAnimator : MonoBehaviour
{
    public Animator Anim { get; private set; }
    // Use this for initialization
    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
    }
}