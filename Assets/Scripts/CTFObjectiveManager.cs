using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CTFObjectiveManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI objectiveText;

    [Header("Display Timing")]
    public float startMessageDuration = 7f;
    public float needEnemiesDuration = 4f;
    public float needFlagDuration = 5f;
    public float victoryMessageDuration = 3f;

    [Header("Messages")]
    [TextArea(3, 5)] public string startMessage = "OBJECTIVES: Eliminate 10 enemies and Capture the flag and bring it to the platform";
    [TextArea(2, 3)] public string needEnemiesMessage = "Eliminate more enemies before capturing the flag!";
    [TextArea(2, 3)] public string needFlagMessage = "Objective complete! Now capture the flag!";
    [TextArea(2, 3)] public string victoryMessage = "MISSION COMPLETE!";

    [Header("Win Settings")]
    public int winSceneIndex = 12;
    public float delayBeforeWin = 3f;

    private bool allEnemiesDead = false;
    private bool flagCaptured = false;
    private bool hasWon = false;
    private bool hasShownEnemyCompleteMessage = false;
    private Coroutine currentMessageCoroutine;

    private GameTimer gameTimer;
    private static CTFObjectiveManager instance;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer == null)
            Debug.LogError("[CTFObjectiveManager] No GameTimer found in scene!");

        StartCoroutine(ShowObjectivesAfterDelay(0.5f));
    }

    IEnumerator ShowObjectivesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowTemporaryMessage(startMessage, startMessageDuration);
    }

    void Update() => CheckObjectives();

    void CheckObjectives()
    {
        bool enemiesDeadNow = EnemyEliminated.AreAllEnemiesDead();
        if (enemiesDeadNow && !allEnemiesDead)
        {
            allEnemiesDead = true;
            OnAllEnemiesDead();
        }

        if (allEnemiesDead && flagCaptured && !hasWon)
            OnVictory();
    }

    void OnAllEnemiesDead()
    {
        if (!hasShownEnemyCompleteMessage)
        {
            hasShownEnemyCompleteMessage = true;
            ShowTemporaryMessage(needFlagMessage, needFlagDuration);
        }
    }

    public void OnFlagPickedUp()
    {
        Debug.Log("[CTFObjectiveManager] Flag picked up");
    }

    public void OnFlagDelivered()
    {
        if (!allEnemiesDead)
        {
            ShowTemporaryMessage(needEnemiesMessage, needEnemiesDuration);
            Debug.Log("[CTFObjectiveManager] Cannot win - enemies still alive!");
        }
        else
        {
            flagCaptured = true;
            Debug.Log("[CTFObjectiveManager] Flag captured!");
        }
    }

    void OnVictory()
    {
        hasWon = true;
        ShowTemporaryMessage(victoryMessage, victoryMessageDuration);
        Debug.Log("[CTFObjectiveManager] VICTORY! Requesting scene " + winSceneIndex);

       
        if (gameTimer != null)
            gameTimer.TriggerSceneLoad(winSceneIndex, delayBeforeWin);
        else
            SceneManager.LoadScene(winSceneIndex);
    }

   

    void ShowTemporaryMessage(string message, float duration)
    {
        if (currentMessageCoroutine != null) StopCoroutine(currentMessageCoroutine);
        currentMessageCoroutine = StartCoroutine(DisplayMessageCoroutine(message, duration));
    }

    IEnumerator DisplayMessageCoroutine(string message, float duration)
    {
        if (objectiveText != null) { objectiveText.text = message; objectiveText.gameObject.SetActive(true); }
        yield return new WaitForSeconds(duration);
        if (objectiveText != null) objectiveText.gameObject.SetActive(false);
        currentMessageCoroutine = null;
    }

    public static CTFObjectiveManager GetInstance() => instance;
    public bool AreAllEnemiesDead() => allEnemiesDead;
    public bool IsFlagCaptured() => flagCaptured;
}