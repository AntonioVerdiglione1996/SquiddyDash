using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraLerp : MonoBehaviour
{

    //public Transform Target;

    //public float smooth;


    public float LerpSpeed = 2f;
    public Transform DefaultTarget;

    private Vector3 cameraArm;

    //private Vector3 CameraPos;
    //private float lerpDuration;
    private Vector3 EndPoint = Vector3.zero;
    private float lerp;

    public BasicEvent StartLerpEvent;
    public BasicEvent StartLerpEvent2;
    private void Awake()
    {
        if (StartLerpEvent)
        {
            StartLerpEvent.OnEventRaised += SetEndPoint;
        }
        if (StartLerpEvent2)
        {
            StartLerpEvent2.OnEventRaised += SetEndPoint;
        }
        SetEndPoint();
    }
    private void OnDestroy()
    {
        if (StartLerpEvent)
        {
            StartLerpEvent.OnEventRaised -= SetEndPoint;
        }
        if (StartLerpEvent2)
        {
            StartLerpEvent2.OnEventRaised -= SetEndPoint;
        }
    }
    void Start()
    {
        cameraArm = new Vector3(0f, 9f, -20f);
        lerp = 0f;
        enabled = true;
        //CameraPos = transform.position;
    }
    public void SetEndPoint()
    {
        if(!DefaultTarget)
        {
            DefaultTarget = FindObjectOfType<CharacterController>().transform.root;
        }
        SetEndPoint(DefaultTarget);
    }
    public void SetEndPoint(Transform transform)
    {
        if (transform)
        {
            EndPoint = transform.position;
            lerp = 0f;
            enabled = true;
        }
    }

    //find another bool for checking if landed (maybe a new event?)
    void LateUpdate()
    {
        //Vector3 currentCamPos = transform.position;
        //bool land = CurrentPlatformForSquiddy.CurrentPlatform != null ? CurrentPlatformForSquiddy.CurrentPlatform.IsLanded : false;
        //Vector3 end = Vector3.zero;

        //if (land/* && currentCamPos.y < Target.position.y*/)
        //{
        //end = Target.position;
        //if (transform.position.y < end.y)
        Vector3 currentPos = transform.position;
        lerp += Time.deltaTime * LerpSpeed;

        transform.position = new Vector3(currentPos.x, Mathf.Lerp(currentPos.y, EndPoint.y + cameraArm.y, lerp), currentPos.z);

        if (lerp > 1f)
        {
            enabled = false;
        }

        //}
    }
}
