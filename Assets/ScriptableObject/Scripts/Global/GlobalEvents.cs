using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Events/GlobalEvents")]
public class GlobalEvents : ScriptableObject
{
    public void GameOverTrigger()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void ClickMenuGameOverButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void OnClickCharSelection()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
    public void SelectLevelByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SelectLevelByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ChangeTimeScale(int value)
    {
        Time.timeScale = value;
    }
    public void ParentToTarget(Transform Parent, Transform Son)
    {
        Son.SetParent(Parent);
    }
   
}
