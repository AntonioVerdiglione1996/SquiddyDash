using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//questa è una classe da usare con find object of type. 
//quando hai un prefab e devi assegnare un audiosource a quel prefab
//a runtime fai awake e fai findobjectoftype<AudioSourceFinder> e assegni il campo
//Source a un audiosource privato nella classe che chiama l audio event.
public class AudioSourceFinder : MonoBehaviour
{
    public AudioSource SourceForTrigger;
    public AudioSource SourceForVocalSayNameOfPowerUp;
}
