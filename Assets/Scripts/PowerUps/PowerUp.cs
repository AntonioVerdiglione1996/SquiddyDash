using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public ScoreSystem ScoreSystem;
    private void OnBecameVisible()
    {
        int random = (int)Random.Range(0, 3);
        transform.position = SpawnPoints[random].position;

    }
    private void OnTriggerEnter(Collider other)
    {
        //squiddy layer
        if (other.gameObject.layer == 8)
        {
            ScoreSystem.Score += 5;

            gameObject.SetActive(false);
        }
    }
}
