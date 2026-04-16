using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EnemyEliminated : MonoBehaviour
{
    [Header("UI Elements")]
    public Text messageText;
    public Text enemyCounterText;

    [Header("Display Timing")]
    public float messageDuration = 2f;

    [Header("Kill Requirement")]
    [Tooltip("How many enemies must be killed to complete the objective")]
    public int requiredKills = 10;

    private static int enemyDeathCount = 0;
    private static int requiredKillCount = 10;
    private static EnemyEliminated instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

       
        requiredKillCount = requiredKills;

        if (messageText != null)
            messageText.gameObject.SetActive(false);

        if (enemyCounterText != null)
        {
            enemyCounterText.gameObject.SetActive(true);
            UpdateCounterUI(); 
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        requiredKillCount = requiredKills;
        ResetCounter();
    }

    public static void ShowEnemyEliminated()
    {
        if (instance == null) return;

        instance.StartCoroutine(instance.HandleEnemyEliminated());
    }

    private IEnumerator HandleEnemyEliminated()
    {
        if (enemyDeathCount >= requiredKillCount)
            yield break;

        enemyDeathCount++;

        if (messageText != null)
        {
            messageText.text = "Enemy eliminated!";
            messageText.gameObject.SetActive(true);
        }

        UpdateCounterUI();

        yield return new WaitForSeconds(messageDuration);

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    private static void UpdateCounterUI()
    {
        if (instance != null && instance.enemyCounterText != null)
        {
            instance.enemyCounterText.text =
                $"Enemies Eliminated: {enemyDeathCount}/{requiredKillCount}";
        }
    }

    public static bool AreAllEnemiesDead()
    {
        return enemyDeathCount >= requiredKillCount;
    }

    public static void ResetCounter()
    {
        enemyDeathCount = 0;
        UpdateCounterUI();
    }

    public static int GetKillCount()
    {
        return enemyDeathCount;
    }

    public static int GetRequiredKills()
    {
        return requiredKillCount;
    }
}