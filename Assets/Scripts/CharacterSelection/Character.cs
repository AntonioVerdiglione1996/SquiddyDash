using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
#if UNITY_EDITOR
    public bool DebugActive = true;
#endif
    public string Name;
    public Color colorName;
    public Sprite Icon;

    [SerializeField]
    private Skill[] Skills = null;

    private SquiddyController controller = null;

    private void Start()
    {
        controller = GetComponentInParent<SquiddyController>();
        if (!controller)
        {
#if UNITY_EDITOR
            if (DebugActive)
            {
                Debug.LogErrorFormat("{0} could not find a valid controller reference!", this);
            }
#endif
            return;
        }

        if (Skills == null || Skills.Length == 0)
        {
            Skills = GetComponentsInChildren<Skill>();
        }

        if (Skills != null)
        {
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i].Initialize(controller);
            }
        }
    }

    private void Update()
    {
        if (Skills != null)
        {
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i].InvokeSkill();
            }
        }
    }
}
