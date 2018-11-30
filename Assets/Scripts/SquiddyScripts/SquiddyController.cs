using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SquiddyController : MonoBehaviour
{
    public GameEvent GameoverEvent;
    public GameEvent GameoverBestScoreSerialization;
    //public GameEvent InstantiateParticleSplash;
    //public GameEvent InstantiateParticleLand;
    [NonSerialized]
    public ParticleSystem Splash;
    public ParticleSystem LandParticle;
    public GameEvent CameraShakeEvent;
    public GameEvent LerpPrerformer;

    public UltimateSkill UltimateSkill;

    public SquiddyStats SquiddyStats;
    public Platform StartPlatform;
    public Rigidbody Rb { get; private set; }

    public bool IsJumping { get { return transform.parent == null; } }


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
        if(!StartPlatform)
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

        if (!UltimateSkill)
        {
            UltimateSkill = GetComponentInChildren<UltimateSkill>();
        }

        if (UltimateSkill)
        {
            UltimateSkill.Initialize(this);
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

        if (UltimateSkill)
        {
            UltimateSkill.InvokeSkill(false);
        }
    }
    public void BotBorderCollision()
    {
        if (GameoverEvent != null)
        {
            //forzo la pulizia dell lista dei pool
            ObjectPooler.OnGameoverPoolClear();
            GameoverEvent.Raise(GameoverBestScoreSerialization);
        }
    }
    public void TopBorderCollision()
    {
        if (LerpPrerformer != null)
            LerpPrerformer.Raise();
    }
    public void BorderCollided()
    {
        if (Splash != null)
            Splash.Play();
        if (CameraShakeEvent != null)
            CameraShakeEvent.Raise();
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
        Vector3 dir = directionSwitcher();
        Rb.AddForce(dir * SquiddyStats.JumpPower, ForceMode.Impulse);
    }
    public void Land()
    {
        Rb.AddForce(-Vector3.up * SquiddyStats.LandForce, ForceMode.VelocityChange);
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
