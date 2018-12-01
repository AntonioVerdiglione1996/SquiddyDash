using UnityEngine;
public class RepulsionModificationStatus
{
    public bool IsValid { get; private set; }
    public bool MultiplyWhenFalling { get; private set; }
    public bool IsRepulsionAbsolute { get; private set; }
    public Vector3 RepulsionMultiplier { get; private set; }
    public RepulsionModificationStatus(bool multiplyWhenFalling, bool isRepulsionAbsolute, Vector3 repulsionMultiplier)
    {
        SetValidity(false);
        SetValues(multiplyWhenFalling, isRepulsionAbsolute, repulsionMultiplier);
    }
    public void SetValidity(bool modified)
    {
        IsValid = modified;
    }
    public void SetValues(bool multiplyWhenFalling, bool isRepulsionAbsolute, Vector3 repulsionMultiplier)
    {
        MultiplyWhenFalling = multiplyWhenFalling;
        IsRepulsionAbsolute = isRepulsionAbsolute;
        RepulsionMultiplier = repulsionMultiplier;
    }
}

[CreateAssetMenu]
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
    void Awake()
    {
        Reset();
    }

    public void SetNewRepulsion(RepulsionModificationStatus status)
    {
        if (!walls || status == null)
        {
            return;
        }

        if (IsCurrentRepulsionAbsolute)
        {
            status.SetValidity(false);
            return;
        }

        ResetRepulsion();

        CurrentMod = status;

        walls.RepulsionMultiplier = CurrentMod.RepulsionMultiplier;
        walls.MultiplyWhenFalling = CurrentMod.MultiplyWhenFalling;

        CurrentMod.SetValidity(true);

        return;
    }
    public void ResetRepulsion()
    {
        if (CurrentMod != null)
        {
            CurrentMod.SetValidity(false);
        }
        CurrentMod = null;

        if (!walls)
        {
            return;
        }

        walls.MultiplyWhenFalling = OriginalMultiplyWhenFalling;
        walls.RepulsionMultiplier = OriginalRepulsionMultiplier;
    }
}
