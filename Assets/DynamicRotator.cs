using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum DynamicRotatorState
{
    NotRotating = 0,
    Rotating,
    LerpToDefault,
}
public class DynamicRotator : MonoBehaviour
{
    public const float MinDurationValue = 0.0f;
    public TimeHelper TimeHelper;

    public Vector2 MinMaxSpeed = new Vector2(-5f, 5f);

    public Vector2 MinMaxRotDuration = new Vector2(0.5f, 2f);
    public Quaternion DefaultRotation = Quaternion.identity;

    public float LerpSpeed = 1f;

    public float CurrentSpeed { get; protected set; }
    public float CurrentDuration { get; protected set; }
    public float CurrentTimeElapsed
    {
        get
        {
            if(randomTimer == null || randomTimer.List == null)
            {
                return 0f;
            }
            TimerData data = randomTimer.Value;
            if (!data.Enabled)
            {
                return 0f;
            }
            return data.ElapsedTime;
        }
    }

    private DynamicRotatorState currentState = DynamicRotatorState.NotRotating;
    private LinkedListNode<TimerData> randomTimer;

    private float lerp = 0f;

    public DynamicRotatorState GetCurrentState()
    {
        return currentState;
    }
    public void ForceNewRotationValues()
    {
#if UNITY_EDITOR
        if (currentState != DynamicRotatorState.Rotating)
        {
            Debug.LogWarningFormat("Warning! {0} is going to switch state outside of the given ChangeCurrentState method! Initial state: {1} , final state : {2}.", this, currentState, DynamicRotatorState.Rotating);
        }
#endif
        currentState = DynamicRotatorState.Rotating;

        TimeHelper.RemoveTimer(randomTimer);
        CurrentDuration = UnityEngine.Random.Range(MinMaxRotDuration.x, MinMaxRotDuration.y);
        randomTimer = TimeHelper.AddTimer(ForceNewRotationValues, CurrentDuration);

        CurrentSpeed = UnityEngine.Random.Range(MinMaxSpeed.x, MinMaxSpeed.y);
    }
    public void ChangeCurrentState(DynamicRotatorState newState)
    {
        if (newState == currentState)
        {
            return;
        }
        TimeHelper.RemoveTimer(randomTimer);
        switch (newState)
        {
            case DynamicRotatorState.NotRotating:
                break;
            case DynamicRotatorState.Rotating:
                ForceNewRotationValues();
                break;
            case DynamicRotatorState.LerpToDefault:
                break;
            default:
#if UNITY_EDITOR
                Debug.LogErrorFormat("{0} is not a valid state for the DynamicRotator {2}.", newState, this);
#endif
                break;
        }

        currentState = newState;
    }
    private void OnValidate()
    {
        MinMaxRotDuration.x = Mathf.Max(MinMaxRotDuration.x, MinDurationValue);
        MinMaxRotDuration.y = Mathf.Max(MinMaxRotDuration.y, MinDurationValue);
        if (MinMaxRotDuration.x > MinMaxRotDuration.y)
        {
            float temp = MinMaxRotDuration.x;
            MinMaxRotDuration.x = MinMaxRotDuration.y;
            MinMaxRotDuration.y = temp;
        }

        if (MinMaxSpeed.x > MinMaxSpeed.y)
        {
            float temp = MinMaxSpeed.x;
            MinMaxSpeed.x = MinMaxSpeed.y;
            MinMaxSpeed.y = temp;
        }

        DefaultRotation = transform.root.rotation;
    }

    private void Update()
    {
        switch (currentState)
        {
            case DynamicRotatorState.NotRotating:
                break;
            case DynamicRotatorState.Rotating:
                Rotate();
                break;
            case DynamicRotatorState.LerpToDefault:
                LerpToDefault();
                break;
            default:
#if UNITY_EDITOR
                Debug.LogErrorFormat("{0} is not a valid state for the DynamicRotator {2}.", currentState, this);
#endif
                break;
        }
    }
    private void Rotate()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += CurrentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);
    }
    private void LerpToDefault()
    {
        if(lerp > 1f)
        {
            return;
        }
        lerp += Time.deltaTime * LerpSpeed;
        Quaternion.Lerp(transform.rotation, DefaultRotation, lerp);
    }
}
