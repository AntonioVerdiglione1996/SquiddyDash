using UnityEngine;
public class RepulsionModificationStatus
{
    public bool IsValid { get { return WallsModifier && WallsModifier.CurrentMod == this; } }
    public bool MultiplyWhenFalling { get; private set; }
    public bool IsRepulsionAbsolute { get; private set; }
    public Vector3 RepulsionMultiplier { get; private set; }
    public WallsModifier WallsModifier { get; private set; }
    
    public RepulsionModificationStatus(WallsModifier wallsModifier, bool multiplyWhenFalling, bool isRepulsionAbsolute, Vector3 repulsionMultiplier)
    {
        WallsModifier = wallsModifier;
        SetValues(multiplyWhenFalling, isRepulsionAbsolute, repulsionMultiplier);
    }
    public void SetValues(bool multiplyWhenFalling, bool isRepulsionAbsolute, Vector3 repulsionMultiplier)
    {
        MultiplyWhenFalling = multiplyWhenFalling;
        IsRepulsionAbsolute = isRepulsionAbsolute;
        RepulsionMultiplier = repulsionMultiplier;
    }
    public void ChangeWallsModifier(WallsModifier newWallsModifier)
    {
        if(WallsModifier == newWallsModifier)
        {
            return;
        }
        if (!WallsModifier)
        {
            WallsModifier = newWallsModifier;
            return;
        }
        if(!newWallsModifier)
        {
            return;
        }

        if (IsValid)
        {
            WallsModifier.ResetRepulsion();
        }

        WallsModifier = newWallsModifier;
    }
}

[CreateAssetMenu(menuName = "Gameplay/WallsModifier")]
public class WallsModifier : ScriptableObject
{
    public Walls Walls
    {
        get
        {
            if (!walls)
            {
                Reset();
            }
            return walls;
        }
    }
    public Vector3 OriginalRepulsionMultiplier { get; private set; }
    public bool OriginalMultiplyWhenFalling { get; private set; }

    public bool IsRepulsionChanged { get { return CurrentMod != null; } }
    public bool IsCurrentRepulsionAbsolute { get { return (IsRepulsionChanged ? CurrentMod.IsRepulsionAbsolute : false); } }

    public RepulsionModificationStatus CurrentMod { get; private set; }

    private Walls walls;

    public void Reset()
    {
        ResetRepulsion();

        walls = FindObjectOfType<Walls>();
        if (walls)
        {
            OriginalRepulsionMultiplier = walls.RepulsionMultiplier;
            OriginalMultiplyWhenFalling = walls.MultiplyWhenFalling;
        }
    }
    public void SetNewRepulsion(RepulsionModificationStatus status)
    {
        if (!Walls || status == null || IsCurrentRepulsionAbsolute)
        {
            return;
        }

        ResetRepulsion();

        status.ChangeWallsModifier(this);

        CurrentMod = status;

        walls.RepulsionMultiplier = CurrentMod.RepulsionMultiplier;
        walls.MultiplyWhenFalling = CurrentMod.MultiplyWhenFalling;

        return;
    }
    public void ResetRepulsion()
    {
        CurrentMod = null;

        if (!walls)
        {
            return;
        }

        walls.MultiplyWhenFalling = OriginalMultiplyWhenFalling;
        walls.RepulsionMultiplier = OriginalRepulsionMultiplier;
    }

    void Awake()
    {
        Reset();
    }
}
