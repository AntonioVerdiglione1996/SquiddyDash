using System.IO;
using UnityEditor;

public class BuildAssetBundles
{
    [MenuItem("Assets/BuildAssetBundlesForWindows")]
    static void BuildAllAssetBundlesWin()
    {
        string assetBundleDirectoryPath = "Assets/AssetBundles";
        string windowsDirectoryPath = "Windows";
        BuildForTarget(assetBundleDirectoryPath, windowsDirectoryPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/BuildAssetBundlesForAndroid")]
    static void BuildAllAssetBundlesAndroid()
    {
        string assetBundleDirectoryPath = "Assets/AssetBundles";
        string androidDirectoryPath = "Android";

        BuildForTarget(assetBundleDirectoryPath, androidDirectoryPath, BuildAssetBundleOptions.None, BuildTarget.Android);
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
