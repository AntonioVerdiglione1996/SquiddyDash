using UnityEngine;
[CreateAssetMenu]
public class GameCurrencyCalculator : ScriptableObject
{
    [SerializeField]
    protected long BaseAmount = 15;
    [SerializeField]
    protected double ScoreMultiplicator = 0.5f;
    public virtual long CalculateCurrencyIncreaseAmount(ScoreSystem ScoreSystem)
    {
        return BaseAmount + (long)(ScoreSystem.Score * ScoreMultiplicator);
    }
}
