using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpawnPlatform : MonoBehaviour
{
    public Transform Squiddy;
    public GameObject PlatformPrefab;
    public GameObject PlatformPrefabWithPowerUp;
    [System.NonSerialized]
    public List<GameObject> PlatformList = new List<GameObject>();
    public Material Material;

    public PositionalEvent OnPlatformSpawn;

    public int PlatformListLength { get { return PlatformList.Count; } }
    public int numberOfObject = 9;

    public Vector3 VectorToAdd = new Vector3(0f,11f,0f);
    public int treshHold = 25;
    private Vector3 newPos;
    private int index;

    private void Awake()
    {
        index = 0;

        newPos = VectorToAdd;

        for (int i = 0; i < numberOfObject; i++)
        {
            //normal platform spawn
            if (i != numberOfObject -1)
            {
                GameObject go = Instantiate(PlatformPrefab, newPos, Quaternion.identity);
                if (Material != null)
                {
                   Renderer[] go_renderers = go.GetComponentsInChildren<Renderer>();
                    for (int j = 0; j < go_renderers.Length; j++)
                    {
                        go_renderers[j].material = Material;
                    }

                }

                PlatformList.Add(go);
                if(OnPlatformSpawn)
                {
                    OnPlatformSpawn.Location = go.transform;
                    OnPlatformSpawn.Raise();
                }
            }
            //powerUp
            else
            {
                GameObject go = Instantiate(PlatformPrefabWithPowerUp, newPos, Quaternion.identity);
                if (Material != null)
                    go.GetComponentInChildren<Renderer>().material = Material;

                PlatformList.Add(go);

                PowerUp pw = go.GetComponentInChildren<PowerUp>(true);
                pw.ResetState();

                if(OnPlatformSpawn)
                {
                    OnPlatformSpawn.Location = go.transform;
                    OnPlatformSpawn.Raise();
                }
            }
            //this will happend even the last cycle. we want that this happend only until the cicle before the last so we do it manually.
            if (i < numberOfObject - 1)
                newPos += VectorToAdd;
        }
    }
    private void Update()
    {
        //resetting part: se squiddy va oltre il treshhold, la piattaforma all'indice corrente si sposta
        if (index <= PlatformList.Count)
        {
            GameObject currentPlatform = PlatformList[index];
            if (currentPlatform.transform.position.y + treshHold < Squiddy.position.y)
            {
                currentPlatform.transform.position += newPos;

                currentPlatform.GetComponentInChildren(typeof(ActivateCollidersOnPlatforms), true).gameObject.SetActive(true);
                //re actiave powerUp on platform 

                //...

                //

                //resetting this bool for perform again the score update for 1 frame after recycle
                currentPlatform.GetComponentInChildren<Platform>().IsAlreadyUpdatedScore = false;
                index++;

                PowerUp pw = currentPlatform.GetComponentInChildren<PowerUp>(true);
                if (pw)
                {
                    pw.ResetState();
                }

                if (index == numberOfObject)
                {
                    index = 0;
                }
            }
        }
    }
}
