using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerJumpingState : MonoBehaviour
{
    public GameEvent IsJumping;
    public GameEvent IsNotJumping;

    public bool RaiseAtStart = true;

    private SquiddyController controller;
    private bool previousJumpingState;
    // Use this for initialization
    void Awake()
    {
        controller = GetComponent<SquiddyController>();

    }
    private void Start()
    {
        previousJumpingState = controller.IsJumping;
        if (RaiseAtStart)
        {
            if (previousJumpingState)
            {
                if (IsJumping)
                {
                    IsJumping.Raise();
                }
                return;
            }
            if (IsNotJumping)
            {
                IsNotJumping.Raise();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsJumping == previousJumpingState)
        {
            return;
        }

        if (previousJumpingState)
        {
            if (IsNotJumping)
            {
                IsNotJumping.Raise();
            }
        }
        else
        {
            if (IsJumping)
            {
                IsJumping.Raise();
            }
        }

        previousJumpingState = !previousJumpingState;
    }
}
