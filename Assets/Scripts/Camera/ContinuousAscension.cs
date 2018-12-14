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
    public CameraShake CamShake;

    public float ShakeAmount = 1f;

    private LinkedListNode<TimerData> timer;

    private Transform myTransform;
    [SerializeField]
    private float prevShakeAmount;

    public void RestartTimer()
    {
        TimeHelper.RemoveTimer(timer);
        timer = TimeHelper.AddTimer(StartAscension, null, TimerDurationBeforeAscension);
    }
    public void StopAscension()
    {
        if (CamShake)
        {
            CamShake.StopShake();
            CamShake.shakeAmount = prevShakeAmount;
        }
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
        if (ScoreSystem.Score <= 0)
        {
            return;
        }
        if (CamShake)
        {
            CamShake.PerformShake(float.MaxValue);
            prevShakeAmount = CamShake.shakeAmount;
            CamShake.shakeAmount = ShakeAmount;
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
        if (CamShake)
        {
            prevShakeAmount = CamShake.shakeAmount;
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
