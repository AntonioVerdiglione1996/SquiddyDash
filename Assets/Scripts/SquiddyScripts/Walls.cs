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

    public GameEvent BorderCollision;
    public GameEvent TopCollision;
    public GameEvent BotCollision;

    public Vector3 RepulsionMultiplier = new Vector3(0.8f, 1.0f, 0f);

    public bool MultiplyWhenFalling = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
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
#endif

    public void LateUpdate()
    {
        transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, transform.position.z);
    }
    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8 && collision.rigidbody) //player layer
        {
            ContactPoint contact = collision.contacts[0];

            if (contact.thisCollider == LeftWall || contact.thisCollider == RightWall)
            {
                PlayerBorderCollision(collision, new Vector3(-collision.impulse.x, 0.0f, 0.0f).normalized);
            }
            else
            {
                PlayerBorderCollision(collision, new Vector3(0f,-collision.impulse.y, 0.0f).normalized);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) //player layer
        {
            float distanceFromTop = TopWall.transform.position.y - other.transform.position.y;
            float distanceFromBot = other.transform.position.y - BotWall.transform.position.y;
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
        if(BotCollision != null)
        {
            BotCollision.Raise();
        }
    }

    private void PlayerTopScreenCollision(Collider other)
    {
        if (TopCollision != null)
        {
            TopCollision.Raise();
        }
    }

    private void PlayerBorderCollision(Collision collision, Vector3 normal)
    {
        Vector3 reflection = Vector3.Reflect(collision.relativeVelocity.normalized, normal);

        if (!MultiplyWhenFalling && collision.relativeVelocity.y < 0)
        {
            collision.rigidbody.AddForce(reflection * collision.relativeVelocity.magnitude * 0.5f, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 Vel = reflection * collision.relativeVelocity.magnitude * 0.5f;
            collision.rigidbody.AddForce(new Vector3(Vel.x * RepulsionMultiplier.x, Vel.y * RepulsionMultiplier.y, Vel.z * RepulsionMultiplier.z), ForceMode.VelocityChange);
        }

        if (BorderCollision != null)
        {
            BorderCollision.Raise();
        }
    }
}
