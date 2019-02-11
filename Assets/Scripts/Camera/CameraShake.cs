using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera cam;

    public float shakeDuration = 0f;

    public float shakeAmount = 9f;
    public float decreaseFactor = 1.0f;

    public BasicEvent ShakeEvent;
    public BasicEvent StopShakeEvent;
    public float DefaultShakeDuration = 0.25f;

    public bool shakeY = true;

    private float originalX;

    public void PerformShake()
    {
        PerformShake(DefaultShakeDuration);
    }
    public void PerformShake(float duration)
    {
        shakeDuration = duration;
        if (shakeDuration > 0f && cam)
        {
            if (enabled)
            {
                StopShake();
            }
            originalX = cam.transform.localPosition.x;
            enabled = true;
        }
    }
    public void StopShake()
    {
        shakeDuration = 0f;
        enabled = false;
        cam.transform.localPosition = new Vector3(originalX, cam.transform.localPosition.y, cam.transform.localPosition.z);
    }
    private void Awake()
    {
        StopShake();
        if (ShakeEvent)
        {
            ShakeEvent.OnEventRaised += PerformShake;
        }
        if (StopShakeEvent)
        {
            StopShakeEvent.OnEventRaised += StopShake;
        }
    }
    private void OnDestroy()
    {
        if (ShakeEvent)
        {
            ShakeEvent.OnEventRaised -= PerformShake;
        }
        if (StopShakeEvent)
        {
            StopShakeEvent.OnEventRaised -= StopShake;
        }
    }

    void Update()
    {
        if (!cam)
        {
#if UNITY_EDITOR
            Debug.LogError("Camera shake component does not have a valid reference to the main camera");
#endif
            StopShake();
        }
        Vector3 originalPos = cam.transform.localPosition;
        originalPos.x += UnityEngine.Random.insideUnitCircle.x * shakeAmount * Time.deltaTime;
        if (shakeY)
            originalPos.y += UnityEngine.Random.insideUnitCircle.y * shakeAmount * Time.deltaTime;
        //i dont want the shake on the x axsis
        cam.transform.localPosition = originalPos;
        shakeDuration -= Time.deltaTime * decreaseFactor;
        if (shakeDuration < 0f)
        {
            StopShake();
        }
    }
}
