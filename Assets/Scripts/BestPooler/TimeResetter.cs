using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeResetter : ISOPoolable
{
    public float LifeTime = 1f;
    public TimeHelper TimeHelper;

    private LinkedListNode<TimerData> timer;
    private void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        timer = TimeHelper.RestartTimer(RecycleObj, null, timer, LifeTime);
    }
    public void RecycleObj()
    {
        TimeHelper.RemoveTimer(timer);
        Recycle();
    }

}
