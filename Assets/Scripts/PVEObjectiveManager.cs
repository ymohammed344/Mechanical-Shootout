using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PVEObjectiveManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI objectiveText;

    [Header("Display Timing")]
    public float startMessageDuration = 7f;
    public float victoryMessageDuration = 3f;

    [Header("Messages")]
    [TextArea(3, 5)]
    public string startMessage = "OBJECTIVE:Eliminate all 10 enemies";
    
    [TextArea(2, 3)]
    public string victoryMessage = "MISSION COMPLETE!";

    [Header("Win Settings")]
    public int winSceneIndex = 12;
    public float delayBeforeWin = 3f;

    private bool allEnemiesDead = false;
    private bool hasWon = false;
    private Coroutine currentMessageCoroutine;

    private static PVEObjectiveManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        StartCoroutine(ShowObjectivesAfterDelay(0.5f));
    }

    IEnumerator ShowObjectivesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowTemporaryMessage(startMessage, startMessageDuration);
    }

    void Update()
    {
        CheckObjectives();
    }

    void CheckObjectives()
    {
        bool enemiesDeadNow = EnemyEliminated.AreAllEnemiesDead();
        
        if (enemiesDeadNow && !allEnemiesDead)
        {
            allEnemiesDead = true;
            OnVictory();
        }
    }

    void OnVictory()
    {
        hasWon = true;
        ShowTemporaryMessage(victoryMessage, victoryMessageDuration);
        Debug.Log("[PVEObjectiveManager] VICTORY! Loading win scene...");
        StartCoroutine(LoadWinScene());
    }

    IEnumerator LoadWinScene()
    {
        yield return new WaitForSeconds(delayBeforeWin);
        SceneManager.LoadScene(winSceneIndex);
    }

    void ShowTemporaryMessage(string message, float duration)
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }
        currentMessageCoroutine = StartCoroutine(DisplayMessageCoroutine(message, duration));
    }

    IEnumerator DisplayMessageCoroutine(string message, float duration)
    {
        if (objectiveText != null)
        {
            objectiveText.text = message;
            objectiveText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration);

        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false);
        }

        currentMessageCoroutine = null;
    }

    public static PVEObjectiveManager GetInstance()
    {
        return instance;
    }

    public bool AreAllEnemiesDead()
    {
        return allEnemiesDead;
    }
}
