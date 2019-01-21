using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimations : MonoBehaviour {

    public bool IsJumping;
    public Animator animator;

    private void Update()
    {
        if (IsJumping)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);

        }
    }
}
