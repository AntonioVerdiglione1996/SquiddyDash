using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public const float InverseMaxByte = 1f / byte.MaxValue;

    public Transform CharacterTransform;
    public Transform LaserTransform;
    public float HeightTollerance = 10f;
    public SOPool Pool;

    public Collider[] Colliders;
    public Renderer[] Renderers;

    public SoundEvent DisablerEvent;
    public BasicEvent EnablerEvent;
    public float DisableDuration = 0.3f;
    public AnimationCurve AdditiveStrenghtCurve;
    public TimeHelper TimeHelper;

    private LinkedListNode<TimerData> timer;
    public void OnEnable()
    {
        if (!CharacterTransform)
        {
            SquiddyController controller = FindObjectOfType<SquiddyController>();
            if (controller)
            {
                CharacterTransform = controller.transform;
            }
        }
        if (DisablerEvent)
        {
            DisablerEvent.OnSoundEvent += Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised += Enable;
        }
    }
    public void OnDisable()
    {
        if (DisablerEvent)
        {
            DisablerEvent.OnSoundEvent -= Disable;
        }
        if (EnablerEvent)
        {
            EnablerEvent.OnEventRaised -= Enable;
        }
    }
    private void Update()
    {
        if(CharacterTransform && CharacterTransform.position.y > LaserTransform.position.y + HeightTollerance)
        {
            TimeHelper.RemoveTimer(timer);
            Pool.Recycle(LaserTransform.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
#if UNITY_EDITOR
            Debug.Log("Dead by lasers.");
#endif
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
        if (Renderers != null)
        {
            for (int i = 0; i < Renderers.Length; i++)
            {
                Renderers[i].enabled = enabled;
            }
        }
    }
    public void Enable()
    {
        SetComponentsActivation(true);
        TimeHelper.RemoveTimer(timer);
    }
    public void Disable(byte Strenght)
    {
        SetComponentsActivation(false);
        timer = TimeHelper.RestartTimer(null, EnablerEvent, timer, DisableDuration + AdditiveStrenghtCurve.Evaluate(Strenght * InverseMaxByte));
    }
}
