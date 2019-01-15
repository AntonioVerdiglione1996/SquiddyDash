using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Levels/Container")]
public class LevelContainer : ScriptableObject
{

    public LevelData[] Datas;
    public void OnEnable()
    {
        OnValidate();
    }
    public void OnValidate()
    {
        if (Datas == null || Datas.Length == 0)
        {
            return;
        }

        List<LevelData> tempList = new List<LevelData>(Datas.Length);
        for(int i = 0; i < Datas.Length; i++)
        {
            LevelData level1 = Datas[i];
            bool unique = true;
            for(int j = i + 1; j < Datas.Length; j++)
            {
                if(Datas[j] == level1)
                {
                    unique = false;
                    break;
                }
            }
            if(unique)
            {
                tempList.Add(level1);
            }
        }

        Datas = tempList.OrderBy(data => data.LevelIndex).ToArray();

        int prevIndex = Datas[0].LevelIndex;
        for (int i = 1; i < Datas.Length; i++)
        {
            int index = Datas[i].LevelIndex;
            if (prevIndex == index)
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat("{0} contains levels with same LevelIndex value! Index: {3} first level: {1}, second level: {2}", this, Datas[i - 1], Datas, index);
#endif
            }
            prevIndex = index;
        }
    }
}
