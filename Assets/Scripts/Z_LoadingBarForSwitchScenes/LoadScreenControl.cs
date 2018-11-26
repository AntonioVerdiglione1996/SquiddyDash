using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreenControl : MonoBehaviour
{
    public GameObject LoadingScreenObj;
    public Slider Slider;
    AsyncOperation asyncOp;
    public void LoadScene(int index)
    {
        StartCoroutine(LoadingScreen(index));
    }
    IEnumerator LoadingScreen(int index)
    {
        LoadingScreenObj.SetActive(true);
        asyncOp = SceneManager.LoadSceneAsync(index);
        asyncOp.allowSceneActivation = false;
        while (!asyncOp.isDone)
        {
            Slider.value = asyncOp.progress;
            //0.9 is the 100% progess
            if (asyncOp.progress == 0.9f)
            {
                Slider.value = 1;
                asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
