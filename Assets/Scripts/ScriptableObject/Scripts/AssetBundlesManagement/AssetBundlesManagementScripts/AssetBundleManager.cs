using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AssetBundleData
{
    public AssetBundle AssetBundle;
    public string[] Dependencies;
    public int ReferencesCount;
}
[CreateAssetMenu(menuName = "AssetBundlesManagement/AssetBundleManager")]
public class AssetBundleManager : ScriptableObject
{
    public AssetBundleRootFolder AssetBundleRootFolder;

    //if Initialise() is called this will be always != null
    private AssetBundleManifest manifest;

    private Dictionary<string, AssetBundleData> loadedBundleByName = new Dictionary<string, AssetBundleData>(100);

    private bool isInitialised;
    private void Awake()
    {
        Initialise();
    }
    private void OnDisable()
    {
        isInitialised = false;
    }
    public void Initialise()
    {
        if (isInitialised) { return; }
        isInitialised = true;

        string manifestBundlePath = AssetBundleRootFolder.GetManifestBundlePath();
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(manifestBundlePath);
        manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        Debug.LogFormat("AssetBundleManager initialised. AssetBundleManifest: '{0}'.", manifest);
    }
    public void LoadAssetBundle(string assetBundleName)
    {
        if (manifest == null && !isInitialised)
            Initialise();

        AssetBundleData assetBundleData;

        if (!loadedBundleByName.TryGetValue(assetBundleName, out assetBundleData))
        {
            string[] dependencies = manifest.GetAllDependencies(assetBundleName);

            for (int i = 0; i < dependencies.Length; i++)
            {
                LoadAssetBundleInternal(dependencies[i]);
            }

            LoadAssetBundleInternal(assetBundleName, dependencies);
        }
        else
        {
            assetBundleData.ReferencesCount++;

            for (int i = 0; i < assetBundleData.Dependencies.Length; i++)
            {
                string dependency = assetBundleData.Dependencies[i];
                loadedBundleByName[dependency].ReferencesCount++;
            }
        }
    }
    //public AssetsBundleUnloader LoadAssetBundleWithUnloader(string assetBundleName)
    //{
    //    if (manifest == null && !isInitialised)
    //        Initialise();

    //    AssetBundleData assetBundleData;

    //    if (!loadedBundleByName.TryGetValue(assetBundleName, out assetBundleData))
    //    {
    //        string[] dependencies = manifest.GetAllDependencies(assetBundleName);

    //        for (int i = 0; i < dependencies.Length; i++)
    //        {
    //            LoadAssetBundleInternal(dependencies[i]);
    //        }

    //        LoadAssetBundleInternal(assetBundleName, dependencies);
    //    }
    //    else
    //    {
    //        assetBundleData.ReferencesCount++;

    //        for (int i = 0; i < assetBundleData.Dependencies.Length; i++)
    //        {
    //            string dependency = assetBundleData.Dependencies[i];
    //            loadedBundleByName[dependency].ReferencesCount++;
    //        }
    //    }
    //    AssetsBundleUnloader unloader = new AssetsBundleUnloader(assetBundleName, this);
    //    return unloader;
    //}
    public void UnloadAssetBundle(string assetBundleName)
    {
        AssetBundleData assetBundleData;

        if (loadedBundleByName.TryGetValue(assetBundleName, out assetBundleData))
        {
            for (int i = 0; i < assetBundleData.Dependencies.Length; i++)
            {
                string dependency = assetBundleData.Dependencies[i];
                UnloadAssetBundleInternal(dependency);
            }

            UnloadAssetBundleInternal(assetBundleName);
        }
        else
        {
            Debug.LogErrorFormat("AssetBundle '{0}' was not loaded", assetBundleName);
        }
    }
    public T LoadAsset<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
    {
        T asset = null;
        AssetBundleData assetBundledata;

        if (loadedBundleByName.TryGetValue(assetBundleName, out assetBundledata))
        {
            asset = assetBundledata.AssetBundle.LoadAsset<T>(assetName);

            if (asset == null)
            {
                Debug.LogErrorFormat("Asset '{0}' does not exists in AssetBundle '{1}'", assetName, assetBundleName);
            }
        }
        else
        {
            Debug.LogErrorFormat("AssetBundle '{0}' not loaded yet", assetBundleName);
        }

        return asset;
    }
    private void LoadAssetBundleInternal(string assetBundleName, string[] dependencies = null)
    {
        AssetBundleData assetBundleData;

        if (!loadedBundleByName.TryGetValue(assetBundleName, out assetBundleData))
        {
            string assetBundlePath = Path.Combine(AssetBundleRootFolder.GetAssetBundlesFolderPlatformPath(), assetBundleName);
            AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath);

            assetBundleData = new AssetBundleData
            {
                AssetBundle = bundle,
                ReferencesCount = 1,
                Dependencies = dependencies == null
                                            ? manifest.GetAllDependencies(assetBundleName)
                                            : dependencies

            };

            loadedBundleByName.Add(assetBundleName, assetBundleData);
        }
        else
        {
            assetBundleData.ReferencesCount++;
        }
    }
    private void UnloadAssetBundleInternal(string assetBundleName)
    {
        AssetBundleData assetBundleData = loadedBundleByName[assetBundleName];
        assetBundleData.ReferencesCount--;

        if (assetBundleData.ReferencesCount <= 0)
        {
            assetBundleData.AssetBundle.Unload(true);

            // TODO: implement an AssetBundleData pool (recyle removed instances)
            loadedBundleByName.Remove(assetBundleName);
        }
    }

}

