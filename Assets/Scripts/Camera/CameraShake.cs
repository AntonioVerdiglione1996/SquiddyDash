using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera cam;

    public float shakeDuration = 0f;

    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPos;

    //this is the method we call from our event system to perform the shake
    //normali shake duration is 0 but when i will collide that i switch it for the amount i chose in the setter
    public void PreformShake(float duration)
    {
        shakeDuration = duration;
    }

    void Update()
    {
        originalPos = cam.transform.localPosition;
        if (shakeDuration > 0)
        {
            originalPos += Random.insideUnitSphere * shakeAmount;
            //i dont want the shake on the x axsis
            originalPos.x = 0;
            cam.transform.localPosition = originalPos;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }

    }
}
