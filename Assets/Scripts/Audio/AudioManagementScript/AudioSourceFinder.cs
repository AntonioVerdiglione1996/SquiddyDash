using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//questa è una classe da usare con find object of type. 
//quando hai un prefab e devi assegnare un audiosource a quel prefab
//a runtime fai awake e fai findobjectoftype<AudioSourceFinder> e assegni il campo
//Source a un audiosource privato nella classe che chiama l audio event.
public class AudioSourceFinder : MonoBehaviour
{
    public bool AddLocalSourcesAtStartup = true;
    public bool AddChildSourcesAtStartup = false;
    public bool AddParentSourcesAtStartup = false;
    public bool AddSourcesInOnValidate = true;
    public List<AudioSource> Sources = new List<AudioSource>();
    private void OnValidate()
    {
        if(AddSourcesInOnValidate)
        {
            Awake();
        }
    }
    private void Awake()
    {
        if (AddLocalSourcesAtStartup)
        {
            AddSources(GetComponents<AudioSource>());
        }
        if (AddChildSourcesAtStartup)
        {
            AddSources(GetComponentsInChildren<AudioSource>(true));
        }
        if (AddParentSourcesAtStartup)
        {
            AddSources(GetComponentsInParent<AudioSource>(true));
        }
    }
    public void AddSources(AudioSource[] sources)
    {
        if(sources == null)
        {
            return;
        }
        for (int i = 0; i < sources.Length; i++)
        {
            AddSource(sources[i]);
        }
    }
    public void AddSource(AudioSource source)
    {
        if (source && !Sources.Contains(source))
        {
            Sources.Add(source);
        }
    }
}
