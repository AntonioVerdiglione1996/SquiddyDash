using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Powerup/Score Increase")]
public class PowerUpScoreIncrease : PowerUpLogic
{
    public ScoreSystem ScoreSystem;
    public int ScoreIncrease;
    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        ScoreSystem.UpdateScore(ScoreIncrease);
    }
}