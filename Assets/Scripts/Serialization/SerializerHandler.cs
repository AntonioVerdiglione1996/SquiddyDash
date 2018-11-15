﻿using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SerializerHandler
{
    public static string PersistentDataDirectoryPath { get { return persistentDataDirectoryPath; } }
    public static string AssetDirectoryPath { get { return assetDirectoryPath; } }
    public static string StreamingAssetsDirectoryPath { get { return streamingAssetsDirectoryPath; } }

    private const string serializedDatafolderName = "SerializedData";
    private static string persistentDataDirectoryPath = Path.Combine(Application.persistentDataPath, serializedDatafolderName);
    private static string assetDirectoryPath = Path.Combine(Application.dataPath, serializedDatafolderName);
    private static string streamingAssetsDirectoryPath = Path.Combine(Application.streamingAssetsPath, serializedDatafolderName);

    #region========================== Json ==============================================================
    public static void SaveJsonFile(string directoryPath, string fileName, string jsonRappresentation)
    {
        //if directory at this pass does not exist , than i create it!
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        //create the effective file at that directory with filename you chose
        File.WriteAllText(Path.Combine(directoryPath, fileName), jsonRappresentation);

        Debug.LogFormat("Serialized at path : {0} , Filename : {1}", Path.Combine(directoryPath, fileName), fileName);
    }
    public static void SaveJsonFromInstance(string directoryPath, string fileName, object instanceToTransformInJson, bool prettyPrint)
    {
        //if directory at this pass does not exist , than i create it!
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string jsonRappresentation = JsonUtility.ToJson(instanceToTransformInJson, prettyPrint);
        //create the effective file at that directory with filename you chose
        File.WriteAllText(Path.Combine(directoryPath, fileName), jsonRappresentation);

        Debug.LogFormat("Serialized at path : {0} , Filename : {1}", Path.Combine(directoryPath, fileName), fileName);
    }
    public static void RestoreObjectFromJson(string directoryPath, string filename, object objToRestore)
    {
        if (File.Exists(Path.Combine(directoryPath, filename)))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine(directoryPath, filename)), objToRestore);
        }
        Debug.LogFormat("DeSerialized at path : {0} , Filename : {1}", Path.Combine(directoryPath, filename), filename);
    }
    #endregion

    #region =======================BYTE SERIALIZATION TO ENHANCE EFFICIENCY===============================
    internal static byte[] objectToByteArray(System.Object instanceToByte)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, instanceToByte);

            return ms.ToArray();
        }
    }
    public static void SaveInByteFile(string directory, string fileName, System.Object Instance)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllBytes(Path.Combine(directory, fileName), objectToByteArray(Instance));

    }
    public static System.Object RestoreFromByteFile(string directory, string fileName)
    {
        byte[] arrBytes = File.ReadAllBytes(Path.Combine(directory, fileName));

        using (MemoryStream memStream = new MemoryStream())
        {
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            System.Object obj = (System.Object)binForm.Deserialize(memStream);

            return obj;
        }
    }
    #endregion
}



