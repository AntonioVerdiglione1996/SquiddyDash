using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Loader : MonoBehaviour
{
    public AssetBundleManager mng;
    public string AssetBundleName;
    public string AssetName;
    //every loading MUST be done in a start function for now! 
    //we have some wierd stuffs if is done on an Awake func
    private void Start()
    {
      
    }
}
