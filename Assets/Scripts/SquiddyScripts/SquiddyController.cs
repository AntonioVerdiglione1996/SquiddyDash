using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquiddyController : MonoBehaviour
{
    public GameEvent GameoverEvent;
    public GameEvent GameoverBestScoreSerialization;
    //public GameEvent InstantiateParticleSplash;
    //public GameEvent InstantiateParticleLand;
    public ParticleSystem Splash;
    public ParticleSystem LandParticle;
    public GameEvent CameraShakeEvent;
    public GameEvent LerpPrerformer;

    public SquiddyStats SquiddyStats;
    public Platform StartPlatform;
    private Rigidbody Rb;

    private bool isJumping;
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }

    //where percentage of the screen is hitted? 
    private Vector3 DirTopRightLow = new Vector3(-0.5f, 0.8f, 0);
    private Vector3 DirTopRightUp = new Vector3(-0.5f, 0.3f, 0);

    private Vector3 DirTopLeftLow = new Vector3(0.5f, 0.8f, 0);
    private Vector3 DirTopLeftUp = new Vector3(0.5f, 0.3f, 0);
    //----------------------------------------------------------
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
        Splash = GetComponentInChildren<ParticleSystem>();
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
    }
    void Update()
    {

        if (!SquiddyStats.IsPhoneDebugging)
        {
            #region classic PC Standalone
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (!isJumping)
                {
                    Jump();

                }
                else if (isJumping)
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
                    if (!isJumping)
                    {
                        Jump();
                    }
                    else if (isJumping)
                    {
                        LandParticle.Play();
                        Land();
                    }
                }
            }
            #endregion
        }

        //Bouncing System----------------------------------------------------------------------------------------------
        //squiddy sta toccando con il lato destro il muro destro(il muro è il bordo del device in questo caso)
        if (transform.position.x + (transform.localScale.x * 0.5f) >= MainCamera.orthographicSize * MainCamera.aspect)
        {
            //transform.position -= Vector3.right * SquiddyStats.BouncyPower * Time.deltaTime;
            Rb.isKinematic = true;
            Rb.isKinematic = false;
            //sono sopra la meta dello schermo partendo da squiddy 
            if (transform.position.y > MainCamera.transform.position.y + MainCamera.aspect)
            {
                //up mid screen
                Rb.AddForce(DirTopRightUp * (SquiddyStats.BouncyPower + 8f), ForceMode.VelocityChange);
            }
            else
            {
                //down mid screen
                Rb.AddForce(DirTopRightLow * (SquiddyStats.BouncyPower + 2f), ForceMode.VelocityChange);
            }
            #region EventsRaised
            if (Splash != null)
                Splash.Play();
            if (CameraShakeEvent != null)
                CameraShakeEvent.Raise();
            #endregion
            //return;
        }
        //squiddy sta toccando con il lato sinistro il muro sinistro(il muro è il bordo del device in questo caso)
        if (transform.position.x - (transform.localScale.x * 0.5f) <= -MainCamera.orthographicSize * MainCamera.aspect)
        {
            Rb.isKinematic = true;
            Rb.isKinematic = false;
            //SE LA POSIZIONE DI SQUIDDY  è MAGGIORE DELLA METà DELLO SCHERMO
            if (transform.position.y > MainCamera.transform.position.y + MainCamera.aspect)
            {
                Rb.AddForce(DirTopLeftUp * (SquiddyStats.BouncyPower + 8f), ForceMode.VelocityChange);
            }
            //MINORE DELLO SCHERMO
            else
            {
                Rb.AddForce(DirTopLeftLow * (SquiddyStats.BouncyPower + 3.5f), ForceMode.VelocityChange);
            }
            #region EventsRaised
            if (Splash != null)
                Splash.Play();
            if (CameraShakeEvent != null)
                CameraShakeEvent.Raise();
            #endregion
            //----------------------------------------------------------------------------------------------------
        }

        //gameover squiddy under the screen 
        if (transform.position.y < MainCamera.transform.position.y - MainCamera.orthographicSize)
        {
            if (GameoverEvent != null)
            {
                //forzo la pulizia dell lista dei pool
                ObjectPooler.OnGameoverPoolClear();
                GameoverEvent.Raise(GameoverBestScoreSerialization);
            }
        }
        //sopra lo schermo 
        if (transform.position.y > MainCamera.transform.position.y + MainCamera.orthographicSize)
        {
            if (LerpPrerformer != null)
                LerpPrerformer.Raise();
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
    private void Jump()
    {
        Vector3 dir = directionSwitcher();
        Rb.AddForce(dir * SquiddyStats.JumpPower, ForceMode.Impulse);
        isJumping = true;
    }
    private void Land()
    {
        Rb.AddForce(-Vector3.up * SquiddyStats.LandForce, ForceMode.VelocityChange);
        isJumping = false;
    }

    private Vector3 returnRightDirection()
    {
        return SquiddyStats.RightDirections[(int)Random.Range(0, 2)];
    }
    private Vector3 returnLeftDirection()
    {
        return SquiddyStats.LeftDirections[(int)Random.Range(0, 2)];
    }
}
