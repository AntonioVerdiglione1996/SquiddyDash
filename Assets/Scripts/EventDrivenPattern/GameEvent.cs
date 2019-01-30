using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Utility/Events/Game")]
public class GameEvent : BasicEvent
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
#if UNITY_EDITOR
        if (AllDebugActive && LocalDebugActive)
        {
            Debug.LogFormat("GameEvent {0} registered eventlistener {1}.", this.name, (listener ? listener.ToString() : "NONE"));
        }
#endif
    }
    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
#if UNITY_EDITOR
        if (AllDebugActive && LocalDebugActive)
        {
            Debug.LogFormat("GameEvent {0} unregistered eventlistener {1}.", this.name, (listener ? listener.ToString() : "NONE"));
        }
#endif
    }

    protected override void ExecuteListeners()
    {
        base.ExecuteListeners();

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            GameEventListener listener = listeners[i];
            if (listener)
            {
#if UNITY_EDITOR
                if (AllDebugActive && LocalDebugActive)
                {
                    Debug.LogFormat("\tEventListener {0} invoked.", listener);
                }
#endif
                listener.OnEventRaised();
            }
            else
            {
                listeners.RemoveAt(i);
#if UNITY_EDITOR
                if (AllDebugActive && LocalDebugActive)
                {
                    Debug.LogWarningFormat("\tEventListener at index {0} forcefully removed.", i);
                }
#endif
            }
        }
    }
}
