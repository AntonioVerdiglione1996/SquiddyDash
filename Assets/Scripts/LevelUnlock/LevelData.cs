using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/LevelData")]
public class LevelData : ScriptableObject
{
    public bool IsUnlocked;
    public int bestScore;
    //restore serialized values
    private void OnEnable()
    {
        
    }
    public void UnlockThisLevel()
    {
        IsUnlocked = true;
    }
    public void Restore()
    {
        //for wrapping serialization
    }
}
