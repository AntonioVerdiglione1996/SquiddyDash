using UnityEngine;
[CreateAssetMenu(menuName = "SavedStats/Currency/Calculator")]
public class GameCurrencyCalculator : ScriptableObject
{
    [SerializeField]
    protected int BaseAmount = 15;
    [SerializeField]
    protected double ScoreMultiplicator = 0.5d;
    public virtual int CalculateCurrencyIncreaseAmount(ScoreSystem ScoreSystem)
    {
        return BaseAmount + (int)(ScoreSystem.Score * ScoreMultiplicator);
    }
}
