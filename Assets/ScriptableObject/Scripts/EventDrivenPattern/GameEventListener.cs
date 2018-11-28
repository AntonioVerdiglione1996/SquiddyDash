using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    //--------REMOVE AFTER-----------
    public string Explain;
    //-------------------------------
    public GameEvent Event;
    public UnityEvent Response;

    public bool InvokeFirstTime = false;

    private bool firstTime = true;
    private void OnEnable()
    {
        Event.RegisterListener(this);
        if(firstTime && InvokeFirstTime)
        {
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
    }
}
