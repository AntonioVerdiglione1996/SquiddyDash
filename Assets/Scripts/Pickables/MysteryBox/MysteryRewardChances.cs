[System.Serializable]
public struct MysteryRewardChances
{
    public MysteryBoxType Type;
    public float Chance;
    public bool ValidChance;
    public MysteryRewardChances(MysteryBoxType type, bool validChance, float chance = 1f)
    {
        Type = type;
        Chance = chance;
        ValidChance = validChance;
    }
}