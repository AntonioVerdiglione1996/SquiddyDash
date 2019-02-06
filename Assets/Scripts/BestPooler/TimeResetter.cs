using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeResetter : MonoBehaviour
{
    public float LifeTime = 1f;
    public SOPool Pool;
    public GameObject Obj;
    public TimeHelper TimeHelper;

    public bool UseObjectPoolerIfPoolIsNotDefined = false;

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
        if (!Obj)
        {
            Obj = gameObject;
        }
        if (Pool)
        {
            Pool.Recycle(Obj);
            return;
        }
        if (UseObjectPoolerIfPoolIsNotDefined)
        {
            ObjectPooler.Recycle(Obj);
        }
    }

}
