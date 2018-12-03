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

    public GameEvent StartRotation;
    public GameEvent ResetRotation;
    public GameEvent StartLerpRotation;

    [SerializeField]
    private Skill[] Skills = null;

    private SquiddyController controller = null;

    private DynamicRotator rotator;

    private void OnEnable()
    {
        if (rotator)
        {
            if (StartRotation)
            {
                StartRotation.OnEventRaised += StartRot;
            }
            if (ResetRotation)
            {
                ResetRotation.OnEventRaised += RestartRot;
            }
            if (StartLerpRotation)
            {
                StartLerpRotation.OnEventRaised += StartLerpRot;
            }
        }
    }
    private void OnDisable()
    {
        if (rotator)
        {
            if (StartRotation)
            {
                StartRotation.OnEventRaised -= StartRot;
            }
            if (ResetRotation)
            {
                ResetRotation.OnEventRaised -= RestartRot;
            }
            if (StartLerpRotation)
            {
                StartLerpRotation.OnEventRaised -= StartLerpRot;
            }
        }
    }
    private void StartRot()
    {
        if (rotator)
        {
            rotator.ChangeCurrentState(DynamicRotatorState.Rotating);
        }
    }
    private void RestartRot()
    {
        if (rotator)
        {
            rotator.TryCalculateNewRotationValues();
        }
    }
    private void StartLerpRot()
    {
        if (rotator)
        {
            rotator.ChangeCurrentState(DynamicRotatorState.LerpToDefault);
        }
    }

    private void Awake()
    {
        rotator = GetComponent<DynamicRotator>();
    }

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
        if (controller)
        {
            if (!controller.IsJumping)
            {
                StartLerpRot();
            }
        }
    }
}
