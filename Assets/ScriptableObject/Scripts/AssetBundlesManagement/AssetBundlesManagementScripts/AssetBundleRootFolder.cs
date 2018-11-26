using System.IO;
using UnityEngine;

public enum AssetBundleRootPath
{
    DataPath,
    StreamingAssetsPath,
    PersistentPath,
}
public enum PlatformFolderTarget
{
    Widnows,
    Android
}
[CreateAssetMenu(menuName = "AssetBundlesManagement/AssetBundleRootFolder")]
public class AssetBundleRootFolder : ScriptableObject
{
    //path were AssetBundles folder is stored (ex: C://Users/MyAccount/Documents/MyProject/Assets if we chose default value)
    [SerializeField]
    private AssetBundleRootPath rootPath = AssetBundleRootPath.DataPath;
    //our platform Folder Name(ex:Windows) we build all bundles each time for different platforms
    [SerializeField]
    private PlatformFolderTarget targetPlatform = PlatformFolderTarget.Widnows;

    //our assetBundles folder
    private string assetBundleFolder = "AssetBundles";

    /// <summary>
    /// Get manifest assetbundle container path. Ready to be estracted 
    /// </summary>
    public string GetManifestBundlePath()
    {
        //the manifest container will be automatically called like our platform folder name
        return getAssetBundlePath(getPlatformPath());
    }
    /// <summary>
    /// Get AssetBundle folder path. Were our platforms folder will be.
    /// </summary>
    public string GetAssetBundlesFolderPath()
    {
        return Path.Combine(getRootPath(), assetBundleFolder);
    }
    /// <summary>
    /// Get Platform folder path inside out AssetBundles folder. Here we have every assetBundles
    /// builded for this platform in particular
    /// </summary>
    public string GetAssetBundlesFolderPlatformPath()
    {
        return Path.Combine(GetAssetBundlesFolderPath(), getPlatformPath());
    }
    private string getAssetBundlePath(string assetBundleName)
    {
        string directory = Path.Combine(getRootPath(), assetBundleFolder);
        return Path.Combine(directory, Path.Combine(getPlatformPath(), assetBundleName));
    }
    private string getRootPath()
    {
        string initialPath = string.Empty;
        if (rootPath == AssetBundleRootPath.DataPath)
            initialPath = Application.dataPath;
        else if (rootPath == AssetBundleRootPath.StreamingAssetsPath)
            initialPath = Application.streamingAssetsPath;
        else if (rootPath == AssetBundleRootPath.PersistentPath)
            initialPath = Application.persistentDataPath;
        return initialPath;
    }
    private string getPlatformPath()
    {
        string toreturn = string.Empty;
        if (targetPlatform == PlatformFolderTarget.Widnows)
            toreturn = "Windows";
        else if (targetPlatform == PlatformFolderTarget.Android)
            toreturn = "Android";
        return toreturn;
    }
}
