using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public Wall[] AllWalls;

    public Camera MainCamera;

    public BasicEvent BorderCollision;
    public BasicEvent CameraShake;
    public BasicEvent CameraLerp;
    public BasicEvent TopCollision;
    public BasicEvent GameOverEvent;

    public Vector3 RepulsionMultiplier = new Vector3(0.6f, 0.8f, 0.6f);
    public Vector3 BotWallMinimumRepulsionForce = new Vector3(0f, 15f, 0f);

    public bool MultiplyWhenFalling = false;

    public ForceMode RepulsionMode = ForceMode.VelocityChange;

    public void SetCollisionTrigger(bool isTrigger, EWallType type)
    {
        for (int i = 0; i < AllWalls.Length; i++)
        {
            Wall wall = AllWalls[i];
            if(wall.Type == type)
            {
                wall.Collider.isTrigger = isTrigger;
                break;
            }
        }
    }
    public void SetAllCollisionTrigger(bool isTrigger, EWallType type)
    {
        for (int i = 0; i < AllWalls.Length; i++)
        {
            Wall wall = AllWalls[i];
            if (wall.Type == type)
            {
                wall.Collider.isTrigger = isTrigger;
            }
        }
    }
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
        if (MainCamera)
        {
            transform.SetPositionAndRotation(new Vector3(0.0f, MainCamera.transform.position.y, MainCamera.transform.position.z), MainCamera.transform.rotation);
        }
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>(true);
        if (bodies != null)
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                Rigidbody body = bodies[i];
                body.isKinematic = true;
                body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
        if (AllWalls == null || AllWalls.Length == 0)
        {
            AllWalls = GetComponentsInChildren<Wall>();
        }
        if (AllWalls != null)
        {
            for (int i = 0; i < AllWalls.Length; i++)
            {
                Wall wall = AllWalls[i];
                wall.Collider.enabled = true;
                switch (wall.Type)
                {
                    case EWallType.None:
                        break;
                    case EWallType.Left:
                        wall.Collider.isTrigger = false;
                        if (MainCamera)
                        {
                            wall.transform.SetPositionAndRotation(new Vector3(MainCamera.transform.position.x - MainCamera.orthographicSize * MainCamera.aspect - wall.Collider.size.x * 0.5f, 0f, 0f), Quaternion.identity);
                        }
                        break;
                    case EWallType.Right:
                        wall.Collider.isTrigger = false;
                        if (MainCamera)
                        {
                            wall.transform.SetPositionAndRotation(new Vector3(MainCamera.transform.position.x + MainCamera.orthographicSize * MainCamera.aspect + wall.Collider.size.x * 0.5f, 0f, 0f), Quaternion.identity);
                        }
                        break;
                    case EWallType.Up:
                        wall.Collider.isTrigger = true;
                        if (MainCamera)
                        {
                            wall.transform.SetPositionAndRotation(new Vector3(0f, MainCamera.transform.position.y + MainCamera.orthographicSize + wall.Collider.size.y * 0.5f, 0f), Quaternion.identity);
                        }
                        break;
                    case EWallType.Down:
                        wall.Collider.isTrigger = true;
                        if (MainCamera)
                        {
                            wall.transform.SetPositionAndRotation(new Vector3(0f, MainCamera.transform.position.y - MainCamera.orthographicSize - wall.Collider.size.y * 0.5f, 0f), Quaternion.identity);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, MainCamera.transform.position.y, transform.position.z);
    }
    public void OnCollisionEntered(Collision collision, Wall hit)
    {
        if (collision.collider.gameObject.layer == 8 && collision.rigidbody) //player layer
        {
            switch (hit.Type)
            {
                case EWallType.None:
                    break;
                case EWallType.Left:
                    PlayerBorderCollision(collision, transform.right);
                    break;
                case EWallType.Right:
                    PlayerBorderCollision(collision, -transform.right);
                    break;
                case EWallType.Up:
                    PlayerBorderCollision(collision, -transform.up);
                    break;
                case EWallType.Down:
                    PlayerBorderCollision(collision, transform.up, BotWallMinimumRepulsionForce);
                    break;
                default:
                    break;
            }
        }
    }
    public void OnTriggerStaying(Collider other, Wall hit)
    {
        if (other.gameObject.layer == 8) //player layer
        {
            switch (hit.Type)
            {
                case EWallType.None:
                    break;
                case EWallType.Left:
                    break;
                case EWallType.Right:
                    break;
                case EWallType.Up:
                    PlayerTopScreenCollision(other);
                    break;
                case EWallType.Down:
                    PlayerBotScreenCollision(other);
                    break;
                default:
                    break;
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
        if(CameraLerp != null)
        {
            CameraLerp.Raise();
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
        if(CameraShake != null)
        {
            CameraShake.Raise();
        }
    }
}
