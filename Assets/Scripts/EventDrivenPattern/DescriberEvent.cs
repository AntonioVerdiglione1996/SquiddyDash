using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Utility/Events/Describer")]
public class DescriberEvent : GameEvent
{
    public IDescriber CurrentDescriber;
    public event Action<IDescriber> OnDescriberEvent;
    private void OnEnable()
    {
        CurrentDescriber = null;
    }
    protected override void ExecuteListeners()
    {
        base.ExecuteListeners();

        if (OnDescriberEvent != null)
        {
            OnDescriberEvent(CurrentDescriber);
#if UNITY_EDITOR
            if (AllDebugActive && LocalDebugActive)
            {
                Debug.Log("\tOnDescriberEvent c# event raised.");
            }
#endif
        }
    }
}
