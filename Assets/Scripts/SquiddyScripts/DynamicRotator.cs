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

    //public TimeHelper TimeHelper;

    public Vector2 MinMaxSpeed = new Vector2(-720f, 720f);

    public float MinAbsoluteSpeed = 250f;

    public Vector2 MinMaxRotDuration = new Vector2(0.5f, 2f);
    public Quaternion DefaultRotation = Quaternion.identity;

    public float LerpSpeed = 10f;


    public float CurrentSpeed { get; protected set; }
    public float CurrentDuration { get; protected set; }
    //public float CurrentTimeElapsed
    //{
    //    get
    //    {
    //        if (randomTimer == null || randomTimer.List == null)
    //        {
    //            return 0f;
    //        }
    //        return randomTimer.Value.ElapsedTime;
    //    }
    //}

    private DynamicRotatorState currentState = DynamicRotatorState.NotRotating;
    //private LinkedListNode<TimerData> randomTimer;

    private float lerp = 0f;

    public void TryCalculateNewRotationValues()
    {
        if (currentState == DynamicRotatorState.Rotating)
        {
            CalculateNewRotationValues();
        }
    }
    public DynamicRotatorState GetCurrentState()
    {
        return currentState;
    }
    public void ChangeCurrentState(DynamicRotatorState newState)
    {
        if (newState == currentState)
        {
            return;
        }
        switch (newState)
        {
            case DynamicRotatorState.NotRotating:
                //TimeHelper.RemoveTimer(randomTimer);
                break;
            case DynamicRotatorState.Rotating:
                CalculateNewRotationValues();
                break;
            case DynamicRotatorState.LerpToDefault:
                lerp = 0f;
                //TimeHelper.RemoveTimer(randomTimer);
                break;
            default:
#if UNITY_EDITOR
                Debug.LogErrorFormat("{0} is not a valid state for the DynamicRotator {2}.", newState, this);
#endif
                break;
        }

        currentState = newState;
    }
    public void ChangeStateToRotate()
    {
        ChangeCurrentState(DynamicRotatorState.Rotating);
    }
    public void ChangeStateToNotRotate()
    {
        ChangeCurrentState(DynamicRotatorState.NotRotating);
    }
    public void ChangeStateToLerpToDefault()
    {
        ChangeCurrentState(DynamicRotatorState.LerpToDefault);
    }

    private void CalculateNewRotationValues()
    {
        currentState = DynamicRotatorState.Rotating;

        //TimeHelper.RemoveTimer(randomTimer);
        CurrentDuration = UnityEngine.Random.Range(MinMaxRotDuration.x, MinMaxRotDuration.y);
        //randomTimer = TimeHelper.AddTimer(CalculateNewRotationValues, CurrentDuration);

        CurrentSpeed = UnityEngine.Random.Range(MinMaxSpeed.x, MinMaxSpeed.y);
        float sign = Mathf.Sign(CurrentSpeed);
        if(Mathf.Abs(CurrentSpeed) < MinAbsoluteSpeed)
        {
            CurrentSpeed = sign * MinAbsoluteSpeed;
        }
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
        if (lerp > 1f)
        {
            ChangeCurrentState(DynamicRotatorState.NotRotating);
            return;
        }
        lerp += Time.deltaTime * LerpSpeed;
        transform.rotation = Quaternion.Lerp(transform.rotation, DefaultRotation, lerp);
    }
}
