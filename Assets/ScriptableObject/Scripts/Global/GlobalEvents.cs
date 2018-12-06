using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Events/GlobalEvents")]
public class GlobalEvents : ScriptableObject
{
    public InGameCurrency GameCurrency;
    public GameCurrencyCalculator Calculator;
#if UNITY_EDITOR
    public bool LocalDebugActive = true;
#endif

    public void IncreaseGameCurrency(ScoreSystem system)
    {
        if (system && Calculator && GameCurrency)
        {
#if UNITY_EDITOR
            long previousGC = GameCurrency.GameCurrency;
#endif
            long amount = Calculator.CalculateCurrencyIncreaseAmount(system);

            bool result = GameCurrency.ModifyGameCurrencyAmount(amount);
#if UNITY_EDITOR
            if (LocalDebugActive)
            {
                Debug.LogFormat("The calculated amount {0} {1} added to the GameCurrency amount!\nPrevious gamecurrency amount: {2} , current amount: {3}.", amount , result ? "was successfully" : "failed to be" , previousGC ,GameCurrency.GameCurrency);
            }
#endif
        }
    }
    public void GameOverTrigger()
    {
        SelectLevelByString("GameOver");
    }
    public void ClickMenuGameOverButton()
    {
        SelectLevelByString("StartMenu");
    }
    public void OnClickCharSelection()
    {
        SelectLevelByString("CharacterSelection");
    }
    public void SelectLevelByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("Requested scene load with name {0}", sceneName);
        }
#endif
    }
    public void SelectLevelByIndex(int index)
    {
        SceneManager.LoadScene(index);
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("Requested scene load with index {0}", index);
        }
#endif
    }
    public void ChangeTimeScale(int value)
    {
#if UNITY_EDITOR
        float previousTimeScale = Time.timeScale;
#endif
        Time.timeScale = value;
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("TimeScale modified from {0} to {1}", previousTimeScale, Time.timeScale);
        }
#endif
    }
    public void ParentToTarget(Transform Parent, Transform Son)
    {
        if (Son && Son != Parent && Son.parent != Parent)
        {
            Son.SetParent(Parent);
#if UNITY_EDITOR
            if (LocalDebugActive)
            {
                Debug.LogFormat("{0} parented to {1}", Son, (Parent ? Parent.ToString() : "NONE"));
            }
#endif
        }
    }

}
