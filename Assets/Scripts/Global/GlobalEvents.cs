using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
[CreateAssetMenu(menuName = "Utility/Events/Global")]
public class GlobalEvents : ScriptableObject
{
    public bool IsGameoverDisabled = false;
    public InGameCurrency GameCurrency;
    public GameCurrencyCalculator Calculator;
    public ScoreSystem System;
    public TimeHelper TimeHelper;
    public BasicEvent GameOverTriggerEvent;
    public BasicEvent GameOverEvent;
    public BasicEvent GameOverInterruptedEvent;

    public float GameoverDelay = 1f;
    public bool IsGameoverOngoing { get; private set; }

#if UNITY_EDITOR
    public bool LocalDebugActive = true;
#endif
    public LevelData CurrentLevel { get { return currentLevel; } }
    [SerializeField]
    private LevelData currentLevel;

    private int bonusScore;
    private int bonusCurrency;

    private LinkedListNode<TimerData> timer;
    public List<MysteryBoxType> CollectedBoxes
    {
        get
        {
            if (collectedBoxes == null)
            {
                collectedBoxes = new List<MysteryBoxType>();
            }
            return collectedBoxes;
        }
    }
    private List<MysteryBoxType> collectedBoxes;
    public void AddCollectedBox(MysteryBoxType type)
    {
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("Added {0} to collected mystery boxes.", type);
        }
#endif
        CollectedBoxes.Add(type);
    }
    public void ClearCollectedBox()
    {
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.Log("Collected mystery boxes cleared");
        }
#endif
        CollectedBoxes.Clear();
    }
    private void OnEnable()
    {
        AddBonusCurrency(-bonusCurrency);
        AddBonusScore(-bonusScore);
        IsGameoverOngoing = false;
        if (GameOverEvent)
        {
            GameOverEvent.OnEventRaised += IncreaseGameCurrency;
        }
        if (GameOverTriggerEvent)
        {
            GameOverTriggerEvent.OnEventRaised += GameOverTrigger;
        }
    }
    private void OnDisable()
    {
        if (GameOverEvent)
        {
            GameOverEvent.OnEventRaised -= IncreaseGameCurrency;
        }
        if (GameOverTriggerEvent)
        {
            GameOverTriggerEvent.OnEventRaised -= GameOverTrigger;
        }
    }
    public void AddBonusScore(int score)
    {
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("Modified current bonus score from {0} to {1}.", bonusScore, bonusScore + score);
        }
#endif
        bonusScore += score;
    }
    public void AddBonusCurrency(int currency)
    {
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("Modified current bonus currency from {0} to {1}.", bonusCurrency, bonusCurrency + currency);
        }
#endif
        bonusCurrency += currency;
    }
    public int GetBonusScore()
    {
        return bonusScore;
    }
    public int GetBonusCurrency()
    {
        return bonusCurrency;
    }
    public void RemoveCurrentLevel()
    {
        SetCurrentLevel(null);
    }
    public void SetCurrentLevel(LevelData Level)
    {
#if UNITY_EDITOR
        if (LocalDebugActive)
        {
            Debug.LogFormat("Setted current level data to {0}, index {1}. Previous level was {2}, index {3}.", Level, Level ? Level.LevelIndex : -1, CurrentLevel, CurrentLevel ? CurrentLevel.LevelIndex : -1);
        }
#endif
        currentLevel = Level;
        ClearCollectedBox();
    }
    public void IncreaseGameCurrency()
    {
        if (System && Calculator && GameCurrency)
        {
#if UNITY_EDITOR
            int previousGC = GameCurrency.GameCurrency;
#endif
            int amount = Calculator.CalculateCurrencyIncreaseAmount(System) + GetBonusCurrency();

            bool result = GameCurrency.ModifyGameCurrencyAmount(amount);
#if UNITY_EDITOR
            if (LocalDebugActive)
            {
                Debug.LogFormat("The calculated amount {0} {1} added to the GameCurrency amount!\nPrevious gamecurrency amount: {2} , current amount: {3}.", amount, result ? "was successfully" : "failed to be", previousGC, GameCurrency.GameCurrency);
            }
#endif
        }
        if (CurrentLevel && System)
        {
            CurrentLevel.TryAddEntry((uint)(System.Score + GetBonusScore()), DateTime.Now.ToLocalTime().ToLongDateString());
        }
        AddBonusCurrency(-bonusCurrency);
        AddBonusScore(-bonusScore);
    }
    public void GameOverTrigger()
    {
        if (!IsGameoverDisabled)
        {
#if UNITY_EDITOR
            if (LocalDebugActive)
            {
                Debug.Log("Gameover started");
            }
#endif
            if (!IsGameoverOngoing)
            {
                GameOverEvent.Raise();
                IsGameoverOngoing = !IsGameoverOngoing;
                timer = TimeHelper.RestartTimer(() => SelectLevelByString("GameOver"), null, timer, GameoverDelay);
            }
        }
        else
        {
#if UNITY_EDITOR
            if (LocalDebugActive)
            {
                Debug.Log("Gameover interrupted");
            }
#endif
            GameOverInterruptedEvent.Raise();
        }
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
        IsGameoverOngoing = false;
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
        IsGameoverOngoing = false;
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
        ChangeTimeScale((float)value);
    }
    public void ChangeTimeScale(float value)
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
    public bool ParentToTarget(Transform Parent, Transform Son)
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
            return true;
        }
        return false;
    }

}
