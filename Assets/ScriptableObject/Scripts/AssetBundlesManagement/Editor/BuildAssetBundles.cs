using System.IO;
using UnityEditor;

public class BuildAssetBundles
{

    [MenuItem("Assets/BuildAssetBundlesForCurrentBuildPlatform")]
    static void BuildAllAssetBundles()
    {
        BuildForTarget(AssetBundleRootFolder.BundlesDirectoryPath, AssetBundleRootFolder.DefaultDirectoryPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
    [MenuItem("Assets/BuildAssetBundlesForWindows")]
    static void BuildAllAssetBundlesWin()
    {
        BuildForTarget(AssetBundleRootFolder.BundlesDirectoryPath, AssetBundleRootFolder.WindowsDirectoryPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/BuildAssetBundlesForAndroid")]
    static void BuildAllAssetBundlesAndroid()
    {
        BuildForTarget(AssetBundleRootFolder.BundlesDirectoryPath, AssetBundleRootFolder.AndroidDirectoryPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
    [MenuItem("Assets/BuildAssetBundlesForIOS")]
    static void BuildAllAssetBundlesIOS()
    {
        BuildForTarget(AssetBundleRootFolder.BundlesDirectoryPath, AssetBundleRootFolder.IOSDirectoryPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
    static void BuildForTarget(string assetBundleDirectoryPath, string targetPlatformDirectoryPath, BuildAssetBundleOptions opt, BuildTarget target)
    {
        if (!Directory.Exists(assetBundleDirectoryPath))
        {
            Directory.CreateDirectory(assetBundleDirectoryPath);
        }
        string temp = Path.Combine(assetBundleDirectoryPath, targetPlatformDirectoryPath);
        if (!Directory.Exists(temp))
        {
            Directory.CreateDirectory(temp);
        }
        BuildPipeline.BuildAssetBundles(temp, opt, target);
    }
}
