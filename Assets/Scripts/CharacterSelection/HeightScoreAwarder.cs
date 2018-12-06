using UnityEngine;
public class HeightScoreAwarder : MonoBehaviour
{
    public float HeightForAward = 15f;
    public int ScoreAward = 1;
    public Transform Target;
    public ScoreSystem ScoreSystem;

    public float MaxHeightReached { get; private set; }

    private float previousHeight;

    private float initialHeight;
    private void Start()
    {
        MaxHeightReached = previousHeight = 0f;
        if (Target)
        {
            initialHeight = Target.position.y;
        }
    }
    void Update()
    {
        if (!Target || !ScoreSystem)
        {
            return;
        }
        float currentHeight = Target.position.y - initialHeight;

        if (currentHeight > MaxHeightReached)
        {
            MaxHeightReached = currentHeight;
            if (currentHeight - previousHeight > HeightForAward)
            {
                ScoreSystem.UpdateScore(ScoreAward);
                previousHeight = currentHeight;
            }
        }
    }
}