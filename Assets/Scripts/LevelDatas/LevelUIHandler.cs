using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIHandler : MonoBehaviour
{
    public LevelData LevelData;

    public Image Locker;

    private Button self;

    private void Awake()
    {
        if (GetComponent<Button>() != null)
            self = GetComponent<Button>();
    }

    private void Update()
    {
        if (LevelData.IsUnlocked)
        {
            Locker.gameObject.SetActive(false);
            self.interactable = true;
        }
        else
        {
            Locker.gameObject.SetActive(true);
            self.interactable = false;
        }
    }
}
