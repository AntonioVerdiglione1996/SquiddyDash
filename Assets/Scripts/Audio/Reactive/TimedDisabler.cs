using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDisabler : MonoBehaviour {
    public BasicEvent DisablerEvent;
    public BasicEvent EnablerEvent;
    public float DisableDuration = 0.3f;
    public GameObject Target;
    public TimeHelper TimeHelper;

    private LinkedListNode<TimerData> timer;
    private void Awake()
    {
        //TODO: si devono levare eventi dopo recycler
        if(DisablerEvent)
        {
            DisablerEvent.OnEventRaised += Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised += Enable;
        }
    }
    private void OnDestroy()
    {
        if (DisablerEvent)
        {
            DisablerEvent.OnEventRaised -= Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised -= Enable;
        }
    }

    public void Enable()
    {
        TimeHelper.RemoveTimer(timer);
        Target.SetActive(true);
    }
    public void Disable()
    {
        Target.SetActive(false);
        TimeHelper.RemoveTimer(timer);
        timer = TimeHelper.AddTimer(EnablerEvent, DisableDuration);
    }
}
