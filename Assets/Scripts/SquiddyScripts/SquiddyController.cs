using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SquiddyController : MonoBehaviour
{
    public GameEvent OnLanding;

    public AudioSource ASource;
    public AudioEvent PlayJumpSound;
    public AudioEvent PlayLandSound;

    [NonSerialized]
    public ParticleSystem Splash;
    public ParticleSystem LandParticle;

    public Skill UltimateSkill;

    public SquiddyStats SquiddyStats;
    public Platform StartPlatform;
    public Rigidbody Rb { get; private set; }

    public bool IsJumping { get { return !(transform.parent); } }

    [SerializeField]
    private ButtonSkillActivator ultimateActivator;

    private Platform currentPlatform;
    private Camera MainCamera;
    private void Start()
    {
        Splash splash = GetComponentInChildren<Splash>();
        if (splash)
        {
            Splash = splash.GetComponent<ParticleSystem>();
        }
#if UNITY_EDITOR
        if (!Splash)
        {
            Debug.LogWarning("Splash particle system not found!!");
        }
#endif
    }
    private void Awake()
    {
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
    void Update()
    {
#if (UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount != 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!IsJumping)
                {
                    Jump();
                }
                else
                {
                    LandParticle.Play();
                    Land();
                }
            }
        }
#elif UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsJumping)
            {
                Jump();
            }
            else
            {
                LandParticle.Play();
                Land();
            }
        }
#else
        throw new Exception("Input not supported for the current platform");
#endif

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
        if (PlayJumpSound != null)
            PlayJumpSound.Play(ASource);
        Vector3 dir = directionSwitcher();
        Rb.AddForce(dir * SquiddyStats.JumpPower, ForceMode.Impulse);
    }
    public void Land()
    {
        if (PlayLandSound != null)
            PlayLandSound.Play(ASource);
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
