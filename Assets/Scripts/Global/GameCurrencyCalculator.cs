using UnityEngine;
[CreateAssetMenu(menuName = "SavedStats/Currency/Calculator")]
public class GameCurrencyCalculator : ScriptableObject
{
    [SerializeField]
    protected long BaseAmount = 15;
    [SerializeField]
    protected double ScoreMultiplicator = 0.5d;
    public virtual long CalculateCurrencyIncreaseAmount(ScoreSystem ScoreSystem)
    {
        return BaseAmount + (long)(ScoreSystem.Score * ScoreMultiplicator);
    }
}
