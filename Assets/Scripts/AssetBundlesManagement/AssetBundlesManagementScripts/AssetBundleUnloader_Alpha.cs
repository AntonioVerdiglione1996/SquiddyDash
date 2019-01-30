using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleUnloader_Alpha : IDisposable
{
    public string AssetBundleName;

    private AssetBundleManager mng;

    public AssetBundleUnloader_Alpha(string assetBundleName, AssetBundleManager mng)
    {
        this.AssetBundleName = assetBundleName;
        this.mng = mng;
        GC.SuppressFinalize(this);
    }
    public void Dispose()
    {
        mng.UnloadAssetBundle(AssetBundleName);
#if UNITY_EDITOR
        Debug.Log("Disposed");
#endif
    }
    ~AssetBundleUnloader_Alpha() {
        Dispose();
#if UNITY_EDITOR
        Debug.Log("Destroy");
#endif

    }
}
