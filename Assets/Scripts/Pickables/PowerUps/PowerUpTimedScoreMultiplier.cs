using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Pickables/Powerup/TimedScoreMultiplier")]
public class PowerUpTimedScoreMultiplier : PowerUpLogic
{
    public ScoreSystem ScoreSystem;
    public TimeHelper TimeHelper;

    public float Duration = 10f;
    public double AdditiveScoreMultiplier = 1.0d;

    private LinkedListNode<TimerData> timer;

    private double usedMultiplier;

    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        if (timer != null && timer.Value.Enabled && timer.Value.ElapsedTime < timer.Value.Duration && TimeHelper.RemoveTimer(timer))
        {
            TimeOver();
        }
        timer = TimeHelper.RestartTimer(TimeOver, null, timer, Duration);
        ScoreSystem.ScoreMultiplier += AdditiveScoreMultiplier;
        usedMultiplier = AdditiveScoreMultiplier;
    }
    private void TimeOver()
    {
        ScoreSystem.ScoreMultiplier -= usedMultiplier;
    }
}
