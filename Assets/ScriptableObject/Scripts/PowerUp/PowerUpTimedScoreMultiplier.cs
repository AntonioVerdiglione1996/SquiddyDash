using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Powerup/TimedScoreMultiplier")]
public class PowerUpTimedScoreMultiplier : PowerUpLogic
{
    public ScoreSystem ScoreSystem;
    public TimeHelper TimeHelper;

    public float Duration = 10f;
    public double Multiplier = 2.0d;

    private LinkedListNode<TimerData> timer;

    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        TimeHelper.RemoveTimer(timer);
        timer = TimeHelper.AddTimer(TimeOver, Duration);
        ScoreSystem.ScoreMultiplier += Multiplier;
    }
    private void TimeOver()
    {
        ScoreSystem.ScoreMultiplier -= Multiplier;
    }
}
