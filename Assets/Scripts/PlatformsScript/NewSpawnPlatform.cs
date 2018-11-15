using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpawnPlatform : MonoBehaviour
{
    public Transform Squiddy;
    public GameObject PlatformPrefab;
    public GameObject PlatformPrefabWithPowerUp;
    public List<GameObject> PlatformPrefabsList;
    public Material Material;
    public GameEvent ScoreUpdater;

    public float PlatformPrefabListLength { get { return PlatformPrefabsList.Count; } }
    int numberOfObject;

    private Vector3 newPos;
    private Vector3 vectorToAdd;
    private float yPos;
    private int treshHold;
    private int index;

    private void Awake()
    {
        index = 0;
        numberOfObject = 9;
        treshHold = 25;
        yPos = 11f;

        newPos = new Vector3(0, yPos, 0);
        vectorToAdd = new Vector3(0, yPos, 0);

        for (int i = 0; i < numberOfObject; i++)
        {
            //normal platform spawn
            if (i != 8)
            {
                GameObject go = Instantiate(PlatformPrefab, newPos, Quaternion.identity);
                if (Material != null)
                    go.GetComponent<Renderer>().material = Material;

                PlatformPrefabsList.Add(go);
            }
            //powerUp
            else
            {
                GameObject go = Instantiate(PlatformPrefabWithPowerUp, newPos, Quaternion.identity);
                if (Material != null)
                    go.GetComponent<Renderer>().material = Material;

                PlatformPrefabsList.Add(go);
            }
            //this will happend even the last cycle. we want that this happend only until the cicle before the last so we do it manually.
            if (i < numberOfObject - 1)
                newPos += vectorToAdd;
        }
    }
    private void Update()
    {
        //resetting part: se squiddy va oltre il treshhold, la piattaforma all'indice corrente si sposta
        if (index <= PlatformPrefabsList.Count)
        {
            GameObject currentPlatform = PlatformPrefabsList[index];
            if (currentPlatform.transform.position.y + treshHold < Squiddy.position.y)
            {
                currentPlatform.transform.position += newPos;

                currentPlatform.GetComponentInChildren(typeof(ActivateCollidersOnPlatforms), true).gameObject.SetActive(true);
                //re actiave powerUp on platform 

                //...

                //

                //resetting this bool for perform again the score update for 1 frame after recycle
                currentPlatform.GetComponent<Platform>().IsAlreadyUpdatedScore = false;
                index++;

                if (index == numberOfObject)
                {
                    index = 0;
                }
            }
        }
    }
}
