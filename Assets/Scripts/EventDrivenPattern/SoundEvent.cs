using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Utility/Events/Sound")]
public class SoundEvent : GameEvent {

    public int CurrentStrenght = 0;
    public event Action<int> OnSoundEvent;
    private void OnEnable()
    {
        CurrentStrenght = 0;
    }
    protected override void ExecuteListeners()
    {
        base.ExecuteListeners();

        if(OnSoundEvent != null)
        {
            OnSoundEvent(CurrentStrenght);
#if UNITY_EDITOR
            if (AllDebugActive && LocalDebugActive)
            {
                Debug.Log("\tOnSoundEventRaised c# event raised.");
            }
#endif
        }
    }
}