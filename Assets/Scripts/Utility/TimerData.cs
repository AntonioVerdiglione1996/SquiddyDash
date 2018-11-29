using System;
public struct TimerData
{
    public bool Enabled;
    public float ElapsedTime;
    public float Duration;
    public GameEvent CallbackEvent;
    public Action CallbackAction;
    public TimerData(GameEvent callback, float duration, bool enabled = true, float elapsedTime = 0f)
    {
        Duration = duration;
        CallbackEvent = callback;
        Enabled = enabled;
        ElapsedTime = elapsedTime;
        CallbackAction = null;
    }
    public TimerData(Action callback, float duration, bool enabled = true, float elapsedTime = 0f)
    {
        Duration = duration;
        CallbackEvent = null;
        Enabled = enabled;
        ElapsedTime = elapsedTime;
        CallbackAction = callback;
    }
    public TimerData(Action callbackAction, GameEvent callbackEvent, float duration, bool enabled = true, float elapsedTime = 0f)
    {
        Duration = duration;
        CallbackEvent = callbackEvent;
        Enabled = enabled;
        ElapsedTime = elapsedTime;
        CallbackAction = callbackAction;
    }
}