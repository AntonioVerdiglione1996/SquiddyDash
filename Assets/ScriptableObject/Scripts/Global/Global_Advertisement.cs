using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

[CreateAssetMenu(menuName = "AdvertisementManager")]
public class Global_Advertisement : ScriptableObject
{
    public void ShowAd()
    {
        if (Advertisement.IsReady())
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = handleAdResult });
    }
    private void handleAdResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Rewarded player with +2 currency");
            //reward for player
        }
        if (result == ShowResult.Skipped)
        {
            //half reward or none
            Debug.Log("not fully watched the add");
        }
        if (result == ShowResult.Failed)
        {
            //none
            Debug.Log("can't show add...check our connection than retry");

        }
    }
}
