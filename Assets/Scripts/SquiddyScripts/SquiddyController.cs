using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SquiddyController : MonoBehaviour
{
    public BasicEvent GameOverEvent;

    public BasicEvent OnLanding;

    public AudioSource ASource;
    public AudioEvent PlayJumpSound;
    public AudioEvent PlayLandSound;

    public BasicEvent CameraShake;

    [NonSerialized]
    public ParticleSystem Splash;
    public ParticleSystem LandParticle;

    public ParticleSystem CircleLowToBig;
    public ParticleSystem CircleBigToLow;

    public Skill UltimateSkill;

    public SquiddyStats SquiddyStats;
    public Platform StartPlatform;
    public Rigidbody Rb { get; private set; }

    public BasicEvent OnClickDown;
    public BasicEvent OnClickUp;
    public BasicEvent OnClickPressed;

    public bool IsJumping { get { return !(transform.parent); } }

    [SerializeField]
    private ButtonSkillActivator ultimateActivator;

    private Platform currentPlatform;
    private Camera MainCamera;

    public BasicEvent OnBorderCollisionEvent;

    private void Start()
    {
        Splash splash = transform.root.GetComponentInChildren<Splash>(true);
        if (splash)
        {
            Splash = splash.GetComponent<ParticleSystem>();
        }
#if UNITY_EDITOR
        if (!Splash)
        {
            if (!splash)
            {
                Debug.LogWarning("Splash component not found!!");
            }
            else
            {
                Debug.LogWarning("Splash particle system not found!!");
            }
        }
#endif
    }
    private void Awake()
    {
        if (OnBorderCollisionEvent)
        {
            OnBorderCollisionEvent.OnEventRaised += BorderCollided;
        }
        SquiddyStats.RightDirections = new Vector3[] { SquiddyStats.topRight, SquiddyStats.LessTopRight };
        SquiddyStats.LeftDirections = new Vector3[] { SquiddyStats.topLeft, SquiddyStats.LessTopLeft };
        Rb = GetComponent<Rigidbody>();

        //t = maxTime;
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
        if (!StartPlatform)
        {
            StartPlatform = FindObjectOfType<StartPlatform>().GetComponent<Platform>();
        }
        //check for the first frame initialization of the current platform
        //TODO: Initialization in another class
        if (CurrentPlatformForSquiddy.CurrentPlatform == null)
        {
            CurrentPlatformForSquiddy.CurrentPlatform = StartPlatform;
            CurrentPlatformForSquiddy.CurrentPlatform.IsLanded = true;
        }



        if (UltimateSkill)
        {
            UltimateSkill.Initialize(this);
            if (!UltimateSkill.IsSkillAutoActivating)
            {
                ultimateActivator.ActivableSkill = UltimateSkill;
                ultimateActivator.gameObject.SetActive(true);
            }
        }

        if (GameOverEvent)
        {
            GameOverEvent.OnEventRaised += DisableRoot;
        }

        if (OnClickDown)
        {
            OnClickDown.OnEventRaised += OnClicked;
        }

    }
    private void DisableRoot()
    {
        this.transform.root.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if(OnClickDown)
        {
            OnClickDown.OnEventRaised -= OnClicked;
        }
        if (GameOverEvent)
        {
            GameOverEvent.OnEventRaised -= DisableRoot;
        }
        if (OnBorderCollisionEvent)
        {
            OnBorderCollisionEvent.OnEventRaised += BorderCollided;
        }
    }
    private void OnValidate()
    {
        if (!ultimateActivator)
        {
            ultimateActivator = GetComponentInChildren<ButtonSkillActivator>();
        }

        if (!StartPlatform)
        {
            StartPlatform startP = FindObjectOfType<StartPlatform>();
            if (startP)
            {
                StartPlatform = startP.GetComponentInChildren<Platform>();
            }
        }
        if (!UltimateSkill)
        {
            UltimateSkill = GetComponentInChildren<UltimateSkill>();
        }
        Rigidbody bd = GetComponent<Rigidbody>();
        if (bd)
        {
            bd.freezeRotation = true;
        }
    }
    public void OnClicked()
    {
        if (!IsJumping)
        {
            CircleLowToBig.Play();
            Jump();
        }
        else
        {
            CircleBigToLow.Play();
            LandParticle.Play();
            Land();
        }
    }
    void Update()
    { 
        if (UltimateSkill && UltimateSkill.IsSkillAutoActivating)
        {
            UltimateSkill.InvokeSkill(false);
        }
    }
    public void BorderCollided()
    {
        if (Splash != null)
        {
            Splash.time = 0f;
            Splash.Play();
        }
    }
    private Vector3 directionSwitcher()
    {
        currentPlatform = CurrentPlatformForSquiddy.CurrentPlatform;
        //when we start the level
        if (currentPlatform == null)
            return Vector3.up;
        //after we have setted a current platform
        else if (currentPlatform.DirLeft)
            return returnLeftDirection();

        else if (currentPlatform.DirRight)
            return returnRightDirection();

        else
            return Vector3.up;
    }
    public void Jump()
    {
        if (PlayJumpSound && ASource)
            PlayJumpSound.Play(ASource);
        if (CameraShake != null)
            CameraShake.Raise();
        Vector3 dir = directionSwitcher();
        Rb.AddForce(dir * SquiddyStats.JumpPower, ForceMode.Impulse);
    }
    public void Land()
    {
        if (PlayLandSound && ASource)
            PlayLandSound.Play(ASource);
        if (CameraShake != null)
            CameraShake.Raise();
        Rb.AddForce(-Vector3.up * SquiddyStats.LandForce, ForceMode.VelocityChange);
        if (OnLanding)
        {
            OnLanding.Raise();
        }
    }

    private Vector3 returnRightDirection()
    {
        return SquiddyStats.RightDirections[UnityEngine.Random.Range(0, 2)];
    }
    private Vector3 returnLeftDirection()
    {
        return SquiddyStats.LeftDirections[UnityEngine.Random.Range(0, 2)];
    }
}
