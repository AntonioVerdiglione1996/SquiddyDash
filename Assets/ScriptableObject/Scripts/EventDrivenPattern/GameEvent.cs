using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
#if UNITY_EDITOR
    public static bool AllDebugActive { get; set; }
    public bool LocalDebugActive = true;
    static GameEvent()
    {
        AllDebugActive = true;
    }
#endif


    public event Action OnEventRaised;
    private List<GameEventListener> listeners = new List<GameEventListener>();
    public void Raise(GameEvent gameEventAfterThis)
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
    public void Raise()
    {
#if UNITY_EDITOR
        if (AllDebugActive && LocalDebugActive)
        {
            Debug.LogFormat("GameEvent {0} raised.", this.name);
        }
#endif

        ExecuteListeners();
    }
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

    private void ExecuteListeners()
    {
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
