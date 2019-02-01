using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Utility/Events/Simple")]
public class BasicEvent : ScriptableObject {

#if UNITY_EDITOR
    public static bool AllDebugActive { get; set; }
    public bool LocalDebugActive = false;
    static BasicEvent()
    {
        AllDebugActive = true;
    }
#endif
    public event Action OnEventRaised;
    public virtual void Raise(BasicEvent gameEventAfterThis)
    {
#if UNITY_EDITOR
        if (AllDebugActive && LocalDebugActive)
        {
            Debug.LogFormat("GameEvent {0} raised with {1} as post event to raise.", this.name, (gameEventAfterThis ? gameEventAfterThis.name : "NONE"));
        }
#endif

        ExecuteListeners();

        if (gameEventAfterThis != null)
        {
            gameEventAfterThis.Raise();
        }
    }
    public virtual void Raise()
    {
#if UNITY_EDITOR
        if (AllDebugActive && LocalDebugActive)
        {
            Debug.LogFormat("GameEvent {0} raised.", this.name);
        }
#endif
        ExecuteListeners();
    }
    protected virtual void ExecuteListeners()
    {
        if (OnEventRaised != null)
        {
            OnEventRaised();
#if UNITY_EDITOR
            if (AllDebugActive && LocalDebugActive)
            {
                Debug.Log("\tOnEventRaised c# event raised.");
            }
#endif
        }
    }
}
