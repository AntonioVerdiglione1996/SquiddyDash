using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveBackground : MonoBehaviour
{
    //take the buffer for this audiosource
    public AudioSource Source;
    //start point for the instantiation of cubes Prefabs
    public Transform StartPos;
    //end point for the instantiation of cubes Prefabs
    public Transform EndPos;
    //cubes prefab
    public GameObject Prefab;
    //prefabs material
    public Material Material;
    //multiplayer for scaling
    public float ScaleMultiplier;
    //Algoritmo di campionamento dei buffer
    public FFTWindow Mode;
    //how many cubes we want to instantiate
    [Range(0, 12)]
    public int numberOfObjects = 12;

    private GameObject[] reactiveCubes;

    //private int samples = 512;
    private float LerpSmooth = 20;

    private static readonly float[] soundData = new float[2048];

    void Awake()
    {
        reactiveCubes = new GameObject[numberOfObjects];
        for (int i = 0; i < numberOfObjects; i++)
        {
            float fraction = (float)i / numberOfObjects;
            reactiveCubes[i] = Instantiate(Prefab, transform);
            reactiveCubes[i].transform.position = Vector3.Lerp(StartPos.position, EndPos.position, fraction);

            if (Material != null)
            {
                reactiveCubes[i].GetComponent<Renderer>().material = Material;
            }
        }

    }
    void Update()
    {
        Source.GetSpectrumData(soundData, 0, Mode);
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < numberOfObjects; i++)
        {
            Transform cube = reactiveCubes[i].transform;

            Vector3 targetScale, originalScale;
            targetScale = originalScale = cube.localScale;

            targetScale.y = soundData[i] * ScaleMultiplier;

            cube.localScale = Vector3.Lerp(originalScale, targetScale, LerpSmooth * deltaTime);
        }
    }
}
