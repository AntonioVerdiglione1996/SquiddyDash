using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    //--------REMOVE AFTER-----------
#if UNITY_EDITOR
    public string Name = null;
    public string Explain;
#endif
    //-------------------------------
    public GameEvent Event;
    public UnityEvent Response;

    public bool InvokeFirstTime = false;

    private bool firstTime = true;
    private void OnValidate()
    {
        if (Name == null || Name.Length == 0)
        {
            Name = Event.name + "Listener";
        }
    }
    private void OnEnable()
    {
        Event.RegisterListener(this);
        if (firstTime && InvokeFirstTime)
        {
#if UNITY_EDITOR
            if (GameEvent.AllDebugActive && Event.LocalDebugActive)
            {
                Debug.LogFormat("{0} listener setted as invoke first time raising event {1}", Name, Event.name);
            }
#endif
            Event.Raise();
        }
        firstTime = false;
    }
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }
    public void OnEventRaised()
    {
        Response.Invoke();
#if UNITY_EDITOR
        if (GameEvent.AllDebugActive && Event.LocalDebugActive)
        {
            Debug.LogFormat("\t\tEventListener {0} invoked with {1} number of persistent methods registered.", Name, Response.GetPersistentEventCount());
            for (int i = 0; i < Response.GetPersistentEventCount(); i++)
            {
                Debug.LogFormat("\t\t\t{0} persistent method invoked at target {1}.", Response.GetPersistentMethodName(i), Response.GetPersistentTarget(i));
            }
        }
#endif
    }
}
