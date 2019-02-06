using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDisabler : MonoBehaviour
{
    public BasicEvent DisablerEvent;
    public BasicEvent EnablerEvent;
    public float DisableDuration = 0.3f;
    public GameObject Target;
    public TimeHelper TimeHelper;

    private LinkedListNode<TimerData> timer;
    public void BindToEvents()
    {
        if (DisablerEvent)
        {
            DisablerEvent.OnEventRaised += Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised += Enable;
        }
    }
    public void UnbindFromEvents()
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
        timer = TimeHelper.RestartTimer(null, EnablerEvent, timer, DisableDuration);
    }
}
