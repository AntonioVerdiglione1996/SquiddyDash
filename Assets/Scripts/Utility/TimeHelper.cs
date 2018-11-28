﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimeHelper : ScriptableObject
{
    private LinkedList<TimerData> timers = new LinkedList<TimerData>();

    public LinkedListNode<TimerData> AddTimer(TimerData data)
    {
        return timers.AddLast(data);
    }

    public LinkedListNode<TimerData> AddTimer(GameEvent callback, float duration)
    {
        return timers.AddLast(new TimerData(callback, duration));
    }

    public void RemoveTimer(LinkedListNode<TimerData> toRemove)
    {
        if(toRemove != null && toRemove.List == timers)
        {
            timers.Remove(toRemove);
        }
    }

    public void RemoveAllTimers()
    {
        timers.Clear();
    }
    /*
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("{0} is duplicated of unique instance {1}! {2} will be destroyed.",this,Instance,this.gameObject);
#endif
            Destroy(this.gameObject);
        }
    }*/
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
                    if (timer.Event != null)
                    {
                        timer.Event.Raise();
                    }
                }
            }

            currentNode = nextNode;
        }
    }
}
