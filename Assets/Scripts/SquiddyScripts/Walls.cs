using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{

    public Rigidbody Body;
    public BoxCollider TopWall;
    public BoxCollider BotWall;
    public BoxCollider LeftWall;
    public BoxCollider RightWall;

    public Camera MainCamera;

    public BasicEvent BorderCollision;
    public BasicEvent TopCollision;
    public BasicEvent GameOverEvent;

    public Vector3 RepulsionMultiplier = new Vector3(0.6f, 0.8f, 0.6f);
    public Vector3 BotWallMinimumRepulsionForce = new Vector3(0f, 15f, 0f);

    public bool MultiplyWhenFalling = false;

    public ForceMode RepulsionMode = ForceMode.VelocityChange;
    private void Start()
    {
        OnValidate();
    }
    private void OnValidate()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
        if (TopWall != null && BotWall != null && LeftWall != null && RightWall != null && Body != null)
        {
            Body.isKinematic = true;
            Body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            LeftWall.enabled = true;
            RightWall.enabled = true;
            TopWall.enabled = true;
            BotWall.enabled = true;

            TopWall.isTrigger = true;
            BotWall.isTrigger = true;
            LeftWall.isTrigger = false;
            RightWall.isTrigger = false;

            if (MainCamera != null)
            {
                transform.SetPositionAndRotation(new Vector3(0.0f, MainCamera.transform.position.y, MainCamera.transform.position.z), MainCamera.transform.rotation);
                LeftWall.transform.SetPositionAndRotation(new Vector3(MainCamera.transform.position.x - MainCamera.orthographicSize * MainCamera.aspect - LeftWall.size.x * 0.5f, 0f, 0f), Quaternion.identity);
                RightWall.transform.SetPositionAndRotation(new Vector3(MainCamera.transform.position.x + MainCamera.orthographicSize * MainCamera.aspect + RightWall.size.x * 0.5f, 0f, 0f), Quaternion.identity);
                TopWall.transform.SetPositionAndRotation(new Vector3(0f, MainCamera.transform.position.y + MainCamera.orthographicSize + TopWall.size.y * 0.5f, 0f), Quaternion.identity);
                BotWall.transform.SetPositionAndRotation(new Vector3(0f, MainCamera.transform.position.y - MainCamera.orthographicSize - BotWall.size.y * 0.5f, 0f), Quaternion.identity);
            }
        }
    }

    public void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, MainCamera.transform.position.y, transform.position.z);
    }
    // Update is called once per frame
//    private void Update()
//    {
//#if UNITY_EDITOR
//        Vector3 start = MainCamera.transform.position + MainCamera.transform.forward * 5f;
//        Vector3 otherPosition = FindObjectOfType<SquiddyController>().transform.position;
//        Debug.Log(start + " , " + (start + Vector3.right * Mathf.Abs((RightWall.transform.position.x - RightWall.size.x * 0.5f) - otherPosition.x)));
//        Debug.DrawLine(start, start + Vector3.right * Mathf.Abs((RightWall.transform.position.x - RightWall.size.x * 0.5f) - otherPosition.x), Color.white);
//        Debug.DrawLine(start, start + Vector3.up * Mathf.Abs((TopWall.transform.position.y - TopWall.size.y * 0.5f) - otherPosition.y));
//#endif
//    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8 && collision.rigidbody) //player layer
        {
            //TODO: verify that these distances aare correct
            Vector3 otherPosition = collision.collider.transform.position;
            float distanceFromTop = Mathf.Abs((TopWall.transform.position.y - TopWall.size.y * 0.5f) - otherPosition.y);
            float distanceFromBot = Mathf.Abs(otherPosition.y - (BotWall.transform.position.y + BotWall.size.y * 0.5f));
            float distanceFromLeft = Mathf.Abs(otherPosition.x - (LeftWall.transform.position.x + LeftWall.size.x * 0.5f));
            float distanceFromRight = Mathf.Abs((RightWall.transform.position.x - RightWall.size.x * 0.5f) - otherPosition.x);

            float minDistance = Mathf.Min(distanceFromBot, distanceFromLeft, distanceFromRight, distanceFromTop);

            if (Mathf.Approximately(minDistance, distanceFromLeft))
            {
                PlayerBorderCollision(collision, transform.right);
            }
            else if (Mathf.Approximately(minDistance, distanceFromRight))
            {
                PlayerBorderCollision(collision, -transform.right);
            }
            else if (Mathf.Approximately(minDistance, distanceFromTop))
            {
                PlayerBorderCollision(collision, -transform.up);
            }
            else
            {
                PlayerBorderCollision(collision, transform.up, BotWallMinimumRepulsionForce);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) //player layer
        {
            Vector3 otherPosition = other.transform.position;
            float distanceFromTop = Mathf.Abs((TopWall.transform.position.y - TopWall.size.y * 0.5f) - otherPosition.y);
            float distanceFromBot = Mathf.Abs(otherPosition.y - (BotWall.transform.position.y + BotWall.size.y * 0.5f));
            if (distanceFromTop < distanceFromBot)
            {
                PlayerTopScreenCollision(other);
            }
            else
            {
                PlayerBotScreenCollision(other);
            }
        }
    }

    private void PlayerBotScreenCollision(Collider other)
    {
        if (GameOverEvent != null)
        {
            this.transform.root.gameObject.SetActive(false);
            GameOverEvent.Raise();
        }
    }

    private void PlayerTopScreenCollision(Collider other)
    {
        if (TopCollision != null)
        {
            TopCollision.Raise();
        }
    }

    private void PlayerBorderCollision(Collision collision, Vector3 normal, Vector3 MinimumRepulsionForce = new Vector3())
    {
        Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normal).normalized;

        Rigidbody other = collision.rigidbody;

        other.AddForce(-other.velocity, ForceMode.VelocityChange);

        Vector3 finalForce;

        if (!MultiplyWhenFalling && collision.relativeVelocity.y < 0)
        {
            finalForce = reflection * collision.relativeVelocity.magnitude;
        }
        else
        {
            Vector3 Vel = reflection * collision.relativeVelocity.magnitude;
            finalForce = new Vector3(Vel.x * RepulsionMultiplier.x, Vel.y * RepulsionMultiplier.y, Vel.z * RepulsionMultiplier.z);
        }

        finalForce = finalForce.MaxAbsoluteValue(MinimumRepulsionForce);

        other.AddForce(finalForce, RepulsionMode);

        if (BorderCollision != null)
        {
            BorderCollision.Raise();
        }
    }
}
