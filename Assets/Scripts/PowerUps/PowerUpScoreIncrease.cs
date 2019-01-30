using UnityEngine;
[CreateAssetMenu(menuName = "Powerup/Score Increase")]
public class PowerUpScoreIncrease : PowerUpLogic
{
    public ScoreSystem ScoreSystem;
    public int ScoreIncrease;
    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        ScoreSystem.UpdateScore(ScoreIncrease);
    }
}