using UnityEngine;
using System.Collections.Generic;
public class ContinuousAscension : MonoBehaviour
{
    public float AscensionSpeed = 2f;
    public bool StartsAsEnabled = false;

    public float TimerDurationBeforeAscension = 2f;

    public TimeHelper TimeHelper;
    public ScoreSystem ScoreSystem;

    public MonoBehaviour OtherMovementBehaviour;

    private LinkedListNode<TimerData> timer;

    private Transform myTransform;

    public void RestartTimer()
    {
        TimeHelper.RemoveTimer(timer);
        timer = TimeHelper.AddTimer(StartAscension, null, TimerDurationBeforeAscension);
    }
    public void StopAscension()
    {
        enabled = false;
        if (OtherMovementBehaviour)
        {
            OtherMovementBehaviour.enabled = !enabled;
        }
    }
    public void StopAndRestartTimer()
    {
        StopAscension();
        RestartTimer();
    }
    public void StartAscension()
    {
        if(ScoreSystem.Score <= 0)
        {
            return;
        }
        enabled = true;
        if (OtherMovementBehaviour)
        {
            OtherMovementBehaviour.enabled = !enabled;
        }
    }
    private void OnValidate()
    {
        enabled = StartsAsEnabled;
        if (OtherMovementBehaviour)
        {
            OtherMovementBehaviour.enabled = !enabled;
        }
    }
    private void Awake()
    {
        myTransform = transform;
        enabled = StartsAsEnabled;
    }
    void LateUpdate()
    {
        Vector3 pos = myTransform.position;
        pos += myTransform.up * AscensionSpeed * Time.deltaTime;
        myTransform.position = pos;
    }
}
