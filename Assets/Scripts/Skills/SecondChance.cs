using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChance : TimedSkill
{
    public NewSpawnPlatform PlatformSpawner
    {
        get
        {
            if (!platformSpawner)
            {
                Awake();
            }
            return platformSpawner;
        }
    }
    public Camera MainCamera
    {
        get
        {
            if (!mainCamera)
            {
                Awake();
            }
            return mainCamera;
        }
    }
    public BasicEvent OnGameover;
    public GlobalEvents GlobalEvents;
    public float MinDistanceFromBotBorder = 1f;
    public Vector3 SpawnOffset = new Vector3(0, 1, 0);
    public float TeleportDelay = 0.5f;
    public BasicEvent OnTeleportEvent;
    private NewSpawnPlatform platformSpawner;
    private Camera mainCamera;
    private LinkedListNode<TimerData> timer;
    private void Awake()
    {
        platformSpawner = FindObjectOfType<NewSpawnPlatform>();
        mainCamera = Camera.main;
    }
    protected override void OnStartSkill()
    {
        base.OnStartSkill();
        if (TimeHelper)
        {
            TimeHelper.RemoveTimer(timer);
        }
        GlobalEvents.IsGameoverDisabled = true;
        OnGameover.OnEventRaised += RepositionPlayerToFirstPlatform;
    }
    protected override void OnStopSkill()
    {
        base.OnStopSkill();
        GlobalEvents.IsGameoverDisabled = false;
        OnGameover.OnEventRaised -= RepositionPlayerToFirstPlatform;
    }
    public void RepositionPlayerToFirstPlatform()
    {
        if (timer != null && timer.Value.Enabled)
        {
            return;
        }

        Controller.gameObject.SetActive(false);
        if (!Controller.IsJumping)
        {
            GlobalEvents.ParentToTarget(null, Controller.transform);
        }
        timer = TimeHelper.RestartTimer(TeleportPlayer, OnTeleportEvent, timer, TeleportDelay);
    }
    private void TeleportPlayer()
    {
        if (!Controller.IsJumping)
        {
            GlobalEvents.ParentToTarget(null, Controller.transform);
        }
        LinkedListNode<Platform> current = platformSpawner.ActivePlatforms.First;
        Platform finalPlat = current.Value;
        float lastHeight = float.MaxValue;
        Bounds cameraBounds = MainCamera.GetBounds();
        float cameraBotHeight = cameraBounds.min.y;
        while (current != null)
        {
            Platform platform = current.Value;
            Bounds platBounds = platform.GetBounds();
            float height = platBounds.max.y;
            if (CurrentPlatformForSquiddy.CurrentPlatform != platform && height > cameraBotHeight + MinDistanceFromBotBorder && height < lastHeight)
            {
                lastHeight = height;
                finalPlat = platform;
            }
            current = current.Next;
        }

        if (!finalPlat)
        {
            GlobalEvents.IsGameoverDisabled = false;
            OnGameover.Raise();
            return;
        }
        Bounds finalBounds = finalPlat.GetBounds();
        Vector3 location = new Vector3(0f, finalBounds.max.y, 0f) + SpawnOffset;
#if UNITY_EDITOR
        Debug.LogWarningFormat("{0} moved to {1} from {2}. Camera bounds: {3}", Controller, location, Controller.transform.position, cameraBounds);
#endif
        //Controller.GetComponent<Rigidbody>().MovePosition(location);
        Controller.transform.position = location;
        Controller.gameObject.SetActive(true);
        this.enabled = false;
    }
    protected override void UpdateBehaviour()
    {
        if (!PlatformSpawner || !MainCamera)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("{0} requires a {1} type in scene to work", this, typeof(NewSpawnPlatform));
#endif
            this.enabled = false;
            return;
        }

    }
}
