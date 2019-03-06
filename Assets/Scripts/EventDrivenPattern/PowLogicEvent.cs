using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Utility/Events/PowerupLogic")]
public class PowLogicEvent : GameEvent
{
    public PowerUpLogic CurrentLogic;
    public event Action<PowerUpLogic> OnPowLogicEvent;
    private void OnEnable()
    {
        CurrentLogic = null;
    }
    protected override void ExecuteListeners()
    {
        base.ExecuteListeners();

        if (OnPowLogicEvent != null)
        {
            OnPowLogicEvent(CurrentLogic);
#if UNITY_EDITOR
            if (AllDebugActive && LocalDebugActive)
            {
                Debug.Log("\tOnPowLogicEvent c# event raised.");
            }
#endif
        }
    }
}
