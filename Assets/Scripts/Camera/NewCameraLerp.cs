using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraLerp : MonoBehaviour
{

    //public Transform Target;

    public float smooth;

    [Range(0.01f, 0.2f)]
    public float damp = 0.08f;

    private Vector3 cameraArm;

    private Vector3 CameraPos;
    //private float lerpDuration;
    private Vector3 EndPoint = Vector3.zero;

    void Start()
    {
        cameraArm = new Vector3(0f, 9f, -20f);
        CameraPos = transform.position;
    }

    public void SetEndPoint(Transform transform)
    {
        EndPoint = transform.position;
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
        CameraPos.y = Mathf.Lerp(transform.position.y, EndPoint.y + cameraArm.y, damp);

        transform.position = CameraPos;

        //}
    }
}
