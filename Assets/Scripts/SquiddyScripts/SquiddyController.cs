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
    public float ScoreRequirementMultIncrementPerInvokation = 1.500001f;

    public SquiddyStats SquiddyStats;
    public Platform StartPlatform;
    public Rigidbody Rb { get; private set; }

    public bool IsJumping { get { return transform.parent == null; } }


    private Platform currentPlatform;
    private Camera MainCamera;
    private void Awake()
    {
        //----------------TODO-------------GET the splash form each model--------------------
        //getto lo splash peculiare per ogni character  // splash layer = 15
        ParticleSystem[] psA = GetComponentsInChildren<ParticleSystem>();
        if (psA != null)
        {
            for (int i = 0; i < psA.Length; i++)
            {
                if (psA[i] != LandParticle)
                {
                    Splash = psA[i];
                    break;
                }
            }
        }
        //Splash = GetComponentInChildren<ParticleSystem>();
        //--------------------------------------------------------------------------------
        SquiddyStats.RightDirections = new Vector3[] { SquiddyStats.topRight, SquiddyStats.LessTopRight };
        SquiddyStats.LeftDirections = new Vector3[] { SquiddyStats.topLeft, SquiddyStats.LessTopLeft };
        Rb = GetComponent<Rigidbody>();

        //t = maxTime;
        MainCamera = Camera.main;
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
        }

    }
    void Update()
    {
        if (!SquiddyStats.IsPhoneDebugging)
        {
            #region classic PC Standalone
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
            #endregion
        }
        else
        {
            #region MobileInput
            if (Input.touches.Length != 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
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
            #endregion
        }

        if (UltimateSkill)
        {
            if(UltimateSkill.InvokeSkill(this, false))
            {
                UltimateSkill.ScoreRequirement = (int)(UltimateSkill.ScoreRequirement * ScoreRequirementMultIncrementPerInvokation);
            }
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
    private void Jump()
    {
        Vector3 dir = directionSwitcher();
        Rb.AddForce(dir * SquiddyStats.JumpPower, ForceMode.Impulse);
    }
    private void Land()
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
