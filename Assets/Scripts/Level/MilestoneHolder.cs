using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Milestones")]
public class MilestoneHolder : ScriptableObject
{
    public ScoreMilestone[] Milestones;
    [Tooltip("Default milestone used when Milestones are either null or they have all been reached. Its score target will be countinously updated to current score")]
    public ScoreMilestone DefaultMilestone = new ScoreMilestone(10, 0f, 1f, Vector3.one);
}
