using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Utility/TimeManager")]
public class TimeHelper : ScriptableObject
{
    private LinkedList<TimerData> timers = new LinkedList<TimerData>();

    public LinkedListNode<TimerData> AddTimer(TimerData data)
    {
        return timers.AddLast(data);
    }

    public LinkedListNode<TimerData> AddTimer(BasicEvent callback, float duration)
    {
        return timers.AddLast(new TimerData(callback, duration));
    }

    public LinkedListNode<TimerData> AddTimer(Action callback, float duration)
    {
        return timers.AddLast(new TimerData(callback, duration));
    }

    public LinkedListNode<TimerData> AddTimer(Action callbackAction, BasicEvent callbackEvent, float duration)
    {
        return timers.AddLast(new TimerData(callbackAction, callbackEvent, duration));
    }
    public LinkedListNode<TimerData> RestartTimer(Action callbackAction, BasicEvent callbackEvent, LinkedListNode<TimerData> toRemove, float duration)
    {
        RemoveTimer(toRemove);
        return AddTimer(callbackAction, callbackEvent, duration);
    }
    public bool RemoveTimer(LinkedListNode<TimerData> toRemove)
    {
        if (toRemove != null && toRemove.List == timers)
        {
            timers.Remove(toRemove);
            return true;
        }
        return false;
    }

    public void RemoveAllTimers()
    {
        timers.Clear();
    }

    public void UpdateTime(float deltaTime)
    {
        LinkedListNode<TimerData> currentNode = timers.First;
        LinkedListNode<TimerData> nextNode = null;

        while (currentNode != null)
        {
            nextNode = currentNode.Next;
            TimerData timer = currentNode.Value;

            if (timer.Enabled)
            {
                timer.ElapsedTime += deltaTime;

                if (timer.Duration > timer.ElapsedTime)
                {
                    currentNode.Value = timer;
                }
                else
                {
                    timer.Enabled = false;
                    currentNode.Value = timer;
                    timers.Remove(currentNode);
                    if (timer.CallbackEvent != null)
                    {
                        timer.CallbackEvent.Raise();
                    }
                    if (timer.CallbackAction != null)
                    {
                        timer.CallbackAction();
                    }
                }
            }

            currentNode = nextNode;
        }
    }
}
