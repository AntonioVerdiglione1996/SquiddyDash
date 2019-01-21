using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera cam;

    public float shakeDuration = 0f;

    public float shakeAmount = 0.15f;
    public float decreaseFactor = 1.0f;

    public BasicEvent BorderShakeEvent;
    public float DefaultShakeDuration = 0.2f;

    public void PerformShake()
    {
        PerformShake(DefaultShakeDuration);
    }
    public void PerformShake(float duration)
    {
        shakeDuration = duration;
        if (shakeDuration > 0f && cam)
        {
            enabled = true;
        }
    }
    public void StopShake()
    {
        shakeDuration = 0f;
        enabled = false;
    }
    private void Awake()
    {
        StopShake();
        if(BorderShakeEvent)
        {
            BorderShakeEvent.OnEventRaised += PerformShake;
        }
    }
    private void OnDestroy()
    {
        if (BorderShakeEvent)
        {
            BorderShakeEvent.OnEventRaised -= PerformShake;
        }
    }

    void Update()
    {
        if(!cam)
        {
#if UNITY_EDITOR
            Debug.LogError("Camera shake component does not have a valid reference to the main camera");
#endif
            StopShake();
        }
        Vector3 originalPos = cam.transform.localPosition;
        originalPos.y += Random.insideUnitSphere.y * shakeAmount;
        //i dont want the shake on the x axsis
        cam.transform.localPosition = originalPos;
        shakeDuration -= Time.deltaTime * decreaseFactor;
        if(shakeDuration < 0f)
        {
            StopShake();
        }
    }
}
