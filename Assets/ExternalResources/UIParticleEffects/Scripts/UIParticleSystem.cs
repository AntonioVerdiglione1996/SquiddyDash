using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UIParticleSystem : MonoBehaviour
{
    public Sprite Particle;

    public float Duration = 5f;

    public bool Looping = true;

    public float Lifetime = 5f;

    public float Speed = 5f;

    public float Size = 1f;
    public bool RandomizeRotation;
    [Range(0.1f, 1f)]
    public float RandomizeMultiplayer = 0.1f;
    public float Rotation = 0f;

    public bool PlayOnAwake = true;

    public float Gravity = -9.81f;

    public float EmissionsPerSecond = 10f;

    public Vector2 EmissionDirection = new Vector2(0, 1f);

    public float EmissionAngle = 90f;

    public Gradient ColorOverLifetime;

    public AnimationCurve SizeOverLifetime;

    public AnimationCurve SpeedOverLifetime;

    [HideInInspector]
    public bool IsPlaying { get; protected set; }

    protected float Playtime = 0f;
    protected Image[] ParticlePool;
    protected int ParticlePoolPointer;

    void Awake()
    {
        if (ParticlePool == null)
            Init();
        if (PlayOnAwake)
            Play();
    }

    private void Init()
    {
        ParticlePoolPointer = 0;
        ParticlePool = new Image[(int)(Lifetime * EmissionsPerSecond * 1.1f + 1)];
        for (int i = 0; i < ParticlePool.Length; i++)
        {

            var gameObject = new GameObject("Particle");
            gameObject.transform.SetParent(transform);
            gameObject.SetActive(false);
            ParticlePool[i] = gameObject.AddComponent<Image>();
            ParticlePool[i].transform.localRotation = Quaternion.identity;
            ParticlePool[i].transform.localPosition = Vector3.zero;
            ParticlePool[i].sprite = Particle;
        }
    }

    public void Play()
    {
        IsPlaying = true;
        StartCoroutine(CoPlay());
    }

    private IEnumerator CoPlay()
    {
        WaitForEndOfFrame wf = new WaitForEndOfFrame();
        Playtime = 0f;
        var particleTimer = 0f;
        while (IsPlaying && (Playtime < Duration || Looping))
        {
            Playtime += Time.deltaTime;
            particleTimer += Time.deltaTime;
            while (particleTimer > 1f / EmissionsPerSecond)
            {
                particleTimer -= 1f / EmissionsPerSecond;
                ParticlePoolPointer = (ParticlePoolPointer + 1) % ParticlePool.Length;
                if (!ParticlePool[ParticlePool.Length - 1 - ParticlePoolPointer].gameObject.activeSelf)
                    StartCoroutine(CoParticleFly(ParticlePool[ParticlePool.Length - 1 - ParticlePoolPointer]));
            }
            yield return wf;
        }
        IsPlaying = false;
    }

    private IEnumerator CoParticleFly(Image particle)
    {
        WaitForEndOfFrame wf = new WaitForEndOfFrame();

        particle.gameObject.SetActive(true);
        particle.transform.localPosition = Vector3.zero;
        var particleLifetime = 0f;

        //get default velocity
        var emissonAngle = new Vector3(EmissionDirection.x, EmissionDirection.y, 0f);
        //apply angle
        emissonAngle = Quaternion.AngleAxis(Random.Range(-EmissionAngle / 2f, EmissionAngle / 2f), Vector3.forward) * emissonAngle;
        //normalize
        emissonAngle.Normalize();

        var gravityForce = Vector3.zero;

        while (particleLifetime < Lifetime)
        {
            particleLifetime += Time.deltaTime;

            //apply gravity
            gravityForce = Vector3.up * Gravity * particleLifetime;

            //set position
            particle.transform.position += emissonAngle * SpeedOverLifetime.Evaluate(particleLifetime / Lifetime) * Speed + gravityForce;

            //set scale
            particle.transform.localScale = Vector3.one * SizeOverLifetime.Evaluate(particleLifetime / Lifetime) * Size;

            //set rortaion
            //particle.transform.localRotation = Quaternion.AngleAxis(Rotation * particleLifetime, Vector3.forward);
            if (!RandomizeRotation)
            {
                particle.transform.localRotation = Quaternion.AngleAxis(Rotation * particleLifetime, Vector3.forward);
            }
            else
            {
                particle.transform.localRotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f) * RandomizeMultiplayer * particleLifetime, Vector3.forward);
            }
            //set color

            particle.color = ColorOverLifetime.Evaluate(particleLifetime / Lifetime);

            yield return wf;
        }

        particle.gameObject.SetActive(false);
    }
}
