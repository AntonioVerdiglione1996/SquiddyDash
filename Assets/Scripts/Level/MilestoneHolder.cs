using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Gameplay/Milestones")]
public class MilestoneHolder : ScriptableObject
{
    public bool Validate = false;
    public ScoreMilestone[] Milestones;
    [Tooltip("Default milestone used when Milestones are either null or they have all been reached. Its score target will be countinously updated to current score")]
    public ScoreMilestone DefaultMilestone = new ScoreMilestone(10, 0f, 1f, Vector3.one);


    void OnValidate()
    {
        if (Validate)
        {
            Validate = !Validate;
            if (Milestones != null && Milestones.Length > 0)
            {
                List<ScoreMilestone> miles = new List<ScoreMilestone>(Milestones.OrderBy(x => x.Score));
                for (int i = miles.Count - 1; i >= 0; i--)
                {
                    if(miles.Count(x => x.Score == miles[i].Score) > 1)
                    {
                        miles.RemoveAt(i);
                    }
                }
                Milestones = miles.ToArray();
            }
        }
    }
}
