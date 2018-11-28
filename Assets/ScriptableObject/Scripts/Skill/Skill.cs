using UnityEngine;
using UnityEngine.UI;
public abstract class Skill : ScriptableObject
{
    public string Description;
    public Image Image;
    public GameObject SkillPrefab;
    public GameObject SpawnedPrefab { get; private set; }
    public abstract bool InvokeSkill(SquiddyController controller, bool bypassIsSkillInvokable);
    public abstract bool IsSkillInvokable(SquiddyController controller);
    public void Initialize(SquiddyController controller)
    {
        if (SpawnedPrefab)
        {
            Destroy(SpawnedPrefab);
        }
        if (SkillPrefab)
        {
            SpawnedPrefab = Instantiate<GameObject>(SkillPrefab);
            SpawnedPrefab.SetActive(false);
        }
        ResetSkill(controller);
    }

    protected abstract void ResetSkill(SquiddyController controller);
}
