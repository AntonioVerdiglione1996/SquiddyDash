﻿using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Pickables/Powerup/Score Increase")]
public class PowerUpScoreIncrease : PowerUpLogic
{
    public ScoreSystem ScoreSystem;
    public int ScoreIncrease;
    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        ScoreSystem.UpdateScore(ScoreIncrease);
    }
}