using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour {

    public CanvasGroup fadeGroup;
    private float loadTime;
    private float minimumLogoTime = 3.0f;

    private void Start()
    {
        //start with a solid color screen than u will fade
        fadeGroup.alpha = 1f;

        //Preload the game files
        // 
         ///////////////////////////////////////////
         //////////////////HERE/////////////////////
         ///////////////////////////////////////////
        //

        if(Time.time < minimumLogoTime)
        {
            loadTime = minimumLogoTime;
        }
        else
        {
            loadTime = Time.time;
        }
    }
    private void Update()
    {
        //fade in
        if (Time.time < minimumLogoTime)
            fadeGroup.alpha = 1 - Time.time;

        //fade out
        if(Time.time > minimumLogoTime && loadTime != 0)
        {
            fadeGroup.alpha = Time.time - minimumLogoTime;
            if(fadeGroup.alpha >= 1)
            {
                SceneManager.LoadScene("StartMenu");
            }
        }
    }
}
