[System.Serializable]
public class LeaderboardEntry
{
    public string Name;
    public uint Score;
    public LeaderboardEntry()
    {
        Score = 0;
        Name = string.Empty;
    }
    public LeaderboardEntry(uint Score, string Name)
    {
        this.Name = Name;
        this.Score = Score;
    }
}