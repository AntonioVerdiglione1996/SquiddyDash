using UnityEngine;
using System.Collections;

public class TimeHelperUpdater : MonoBehaviour
{
    public TimeHelper TimeHelper;
    public bool ClearTimersOnAwake = true;
    void Awake()
    {
        if (ClearTimersOnAwake)
        {
            TimeHelper.RemoveAllTimers();
        }
    }
    void Update()
    {
        TimeHelper.UpdateTime(Time.deltaTime);
    }
}
