using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Utility/Events/Positional")]
public class PositionalEvent : GameEvent {
    public Transform Location;
    public event Action<Transform> OnPositionalRaised;
    protected override void ExecuteListeners()
    {
        base.ExecuteListeners();

        if (OnPositionalRaised != null)
        {
            OnPositionalRaised(Location);
#if UNITY_EDITOR
            if (AllDebugActive && LocalDebugActive)
            {
                Debug.Log("\tOnPositionalRaised c# event raised.");
            }
#endif
        }
    }
}
