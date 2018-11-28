public struct TimerData
{
    public bool Enabled { get; set; }
    public float ElapsedTime { get; set; }
    public float Duration { get; set; }
    public GameEvent Event { get; set; }
    public TimerData(GameEvent callback, float duration)
    {
        Duration = duration;
        Event = callback;
        Enabled = true;
        ElapsedTime = 0f;
    }
}