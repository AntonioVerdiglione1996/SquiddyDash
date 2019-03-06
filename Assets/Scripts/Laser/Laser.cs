using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ISOPoolable
{
    public const float DisabledTimerValue = float.MaxValue;
    public const float InverseMaxByte = 1f / byte.MaxValue;

    public SquiddyController CharacterController;
    public float HeightTollerance = 10f;

    public Collider[] Colliders;

    public SoundEvent DisablerEvent;
    public BasicEvent EnablerEvent;
    public BasicEvent GameOverTrigger;
    public BasicEvent GameoverEvent;
    public float DisableDuration = 0.7f;
    public AnimationCurve AdditiveStrenghtCurve;
    public ParticleSystem LaserParticle;
    public GameObject OnPlayerDeathParticle;

    public float CurrentDuration { get { return DisableDuration + AdditiveStrenghtCurve.Evaluate(strenght * InverseMaxByte); } }
    public bool IsDisabled { get { return Mathf.Approximately(timer, DisabledTimerValue); } }

    private float timer = DisabledTimerValue;
    private byte strenght = 0;
    protected override void OnValidate()
    {
        base.OnValidate();
        if (!CharacterController)
        {
            CharacterController = FindObjectOfType<SquiddyController>();
        }
    }
    private void SpawnDeathParticle()
    {
        if (OnPlayerDeathParticle)
        {
            Transform charTransform = CharacterController.transform;
            ParticleSystem system = Instantiate(OnPlayerDeathParticle, charTransform.position, charTransform.rotation).GetComponentInChildren<ParticleSystem>();
            if (system)
            {
                system.Play(true);
            }
        }
    }
    public void OnEnable()
    {
        if (!CharacterController)
        {
            CharacterController = FindObjectOfType<SquiddyController>();
        }
        if (DisablerEvent)
        {
            DisablerEvent.OnSoundEvent += Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised += Enable;
        }
        if (GameoverEvent)
        {
            GameoverEvent.OnEventRaised += SpawnDeathParticle;
        }
        Enable();
    }
    public void OnDisable()
    {
        Disable(0, false);
        if (DisablerEvent)
        {
            DisablerEvent.OnSoundEvent -= Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised -= Enable;
        }
        if (GameoverEvent)
        {
            GameoverEvent.OnEventRaised -= SpawnDeathParticle;
        }
    }
    private void Update()
    {
        if (!IsDisabled)
        {
            timer += Time.deltaTime;
            if (timer >= CurrentDuration)
            {
                Enable();
            }
        }
        if (CharacterController && CharacterController.transform.position.y > Root.transform.position.y + HeightTollerance)
        {
            Recycle();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (CharacterController && !CharacterController.IsInvincible && collision.gameObject.layer == 8)
        {
            this.enabled = false;

            if (GameOverTrigger)
            {
                GameOverTrigger.Raise();
            }
        }
    }

    public void SetComponentsActivation(bool enabled)
    {
        if (Colliders != null)
        {
            for (int i = 0; i < Colliders.Length; i++)
            {
                Colliders[i].enabled = enabled;
            }
        }
    }
    public void Enable()
    {
        if (LaserParticle)
        {
            LaserParticle.time = 0;
            LaserParticle.Play(true);
        }
        SetComponentsActivation(true);
        timer = DisabledTimerValue;
    }
    public void Disable(byte Strenght)
    {
        Disable(Strenght, true);
    }
    public void Disable(byte Strenght, bool restart)
    {
        if (restart && timer < DisableDuration)
        {
            return;
        }
        if (LaserParticle)
        {
            LaserParticle.time = 0;
            LaserParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        SetComponentsActivation(false);
        if (restart)
        {
            timer = 0f;
            strenght = Strenght;
        }
        else
        {
            timer = DisabledTimerValue;
        }
    }
}
