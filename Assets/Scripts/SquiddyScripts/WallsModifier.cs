using UnityEngine;
[CreateAssetMenu]
public class WallsModifier : ScriptableObject
{
    public Walls Walls
    {
        get
        {
            if (!walls)
            {
                Awake();
            }
            return walls;
        }
    }
    public Vector3 OriginalRepulsionMultiplier { get; private set; }
    public bool OriginalMultiplyWhenFalling { get; private set; }

    public bool IsRepulsionChanged { get; private set; }
    public bool IsCurrentRepulsionAbsolute { get; private set; }

    private Walls walls;

    void Awake()
    {
        walls = FindObjectOfType<Walls>();
        if (walls)
        {
            OriginalRepulsionMultiplier = walls.RepulsionMultiplier;
            OriginalMultiplyWhenFalling = walls.MultiplyWhenFalling;
        }
        IsRepulsionChanged = false;
        IsCurrentRepulsionAbsolute = false;
    }

    public bool SetNewRepulsion(Vector3 newRepulsion, bool multiplyWhenFalling, bool isRepulsionAbsolute)
    {
        IsRepulsionChanged = true;
        IsCurrentRepulsionAbsolute = isRepulsionAbsolute;

        if (!walls || IsCurrentRepulsionAbsolute)
        {
            return false;
        }

        walls.RepulsionMultiplier = newRepulsion;
        walls.MultiplyWhenFalling = multiplyWhenFalling;

        return true;
    }
    public void ResetRepulsion()
    {
        IsCurrentRepulsionAbsolute = false;
        IsRepulsionChanged = false;
        if (!walls)
        {
            return;
        }
        walls.MultiplyWhenFalling = OriginalMultiplyWhenFalling;
        walls.RepulsionMultiplier = OriginalRepulsionMultiplier;
    }
}
