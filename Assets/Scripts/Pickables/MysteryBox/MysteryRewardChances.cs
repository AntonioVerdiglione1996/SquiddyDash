[System.Serializable]
public struct MysteryRewardChances
{
    public MysteryBoxType Type;
    public float Chance;
    public MysteryRewardChances(MysteryBoxType type, float chance = 1f)
    {
        Type = type;
        Chance = chance;
    }
}