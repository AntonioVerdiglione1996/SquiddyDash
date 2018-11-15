using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//behaviur that go on the button for opening chest 
//chest reopen in realtime after the chosen time.
//this script will work between sessions so if u want to wait 12 h. after u log in after the msToWait amout
//the button will be again ready 
public class RealTimeTimerButtonBehaviour : MonoBehaviour
{
    // 5 seconds = 5000 ms --->30 min = 1800000 ms ---> 1h = 3600000 ms ---> 12h = 43200000 ms
    public float msToWait = 5000.0f;

    private Button owner;
    private Text timerText;
    private ulong lastClickTime;
    private void Start()
    {
        owner = GetComponent<Button>();
        timerText = GetComponentInChildren<Text>();
        lastClickTime = ulong.Parse(PlayerPrefs.GetString("LastClick"));
        if (!isReadyAgain())
            owner.interactable = false;
    }
    private void Update()
    {
        if (!owner.interactable)
        {
            if (isReadyAgain())
            {
                owner.interactable = true;
                return;
            }

            //set the timer
            ulong diff = (ulong)DateTime.Now.Ticks - lastClickTime;
            ulong ms = diff / TimeSpan.TicksPerMillisecond;

            float secondLeft = (float)(msToWait - ms) / 1000.0f;
            string r = "";
            //hours 
            r += ((int)secondLeft / 3600).ToString() + "h ";

            // this will remove all the hours we have
            secondLeft -= ((int)secondLeft / 3600) * 3600;

            //minutes
            r += ((int)secondLeft / 60).ToString("00") + "m ";

            //seconds
            r += ((int)secondLeft % 60).ToString("00") + "s ";

            timerText.text = r;
        }
    }
    public void OnClick()
    {
        lastClickTime = (ulong)DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastClick", lastClickTime.ToString());
        owner.interactable = false;

        //here u will give the user his reward
    }
    private bool isReadyAgain()
    {
        ulong diff = (ulong)DateTime.Now.Ticks - lastClickTime;
        ulong ms = diff / TimeSpan.TicksPerMillisecond;

        float secondLeft = (float)(msToWait - ms) / 1000.0f;
        if (secondLeft < 0)
        {
            timerText.text = "Ready!";
            return true;
        }
        return false;
    }
}
