using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Utility/Events/Sound")]
public class SoundEvent : GameEvent {

    public int CurrentStrenght = 0;
    private void OnEnable()
    {
        CurrentStrenght = 0;
    }
}