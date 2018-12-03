﻿using System.Collections;
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

    private double usedMultiplier;

    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        if (TimeHelper.RemoveTimer(timer))
        {
            TimeOver();
        }
        timer = TimeHelper.AddTimer(TimeOver, Duration);
        ScoreSystem.ScoreMultiplier += Multiplier;
        usedMultiplier = Multiplier;
    }
    private void TimeOver()
    {
        ScoreSystem.ScoreMultiplier -= usedMultiplier;
    }
}
