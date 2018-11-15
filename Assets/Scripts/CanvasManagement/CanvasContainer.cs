using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CanvasCustom
{
    public int Key;
    public Canvas Value;
}
public class CanvasContainer : MonoBehaviour
{
    [Header("Chose the key and the relative canvas")]
    public List<CanvasCustom> fill = new List<CanvasCustom>();

    private Dictionary<int, Canvas> canvases;

    private void Awake()
    {
        canvases = new Dictionary<int, Canvas>();
        for (int i = 0; i < fill.Count; i++)
        {
            int currentKey = fill[i].Key;
            Canvas currentCanvas = fill[i].Value;

            if (!canvases.ContainsKey(currentKey))
            {
                canvases.Add(currentKey, currentCanvas);
            }
        }
    }
    public void ActivateCurrentCavas(int Key)
    {
        Deactivator();
        //deactivation of all other
        if (!canvases.ContainsKey(Key))
            return;
        canvases[Key].gameObject.SetActive(true);
    }
    private void Deactivator()
    {
        foreach (KeyValuePair<int, Canvas> item in canvases)
        {
            item.Value.gameObject.SetActive(false);
        }
    }
}
