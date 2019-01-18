using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimations : MonoBehaviour {

    public bool bol;
    public Animator animator;

    private void Update()
    {
        if (bol)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);

        }
    }
}
