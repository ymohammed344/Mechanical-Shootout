using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 600f;

    [Header("UI Reference")]
    public Text timerText;

    [Header("Scene Management")]
    public int timeUpSceneIndex = 13;

    private float currentTime;
    private bool timerActive = true;
    private bool sceneLoadTriggered = false; 

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!timerActive) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            timerActive = false;
            OnTimeUp();
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    void OnTimeUp()
    {
        Debug.Log("[GameTimer] Time's up! Loading scene " + timeUpSceneIndex);
        TriggerSceneLoad(timeUpSceneIndex, 3f);
    }


    public void TriggerSceneLoad(int sceneIndex, float delay)
    {
        if (sceneLoadTriggered) return; 
        sceneLoadTriggered = true;
        timerActive = false;
        StartCoroutine(LoadSceneAfterDelay(sceneIndex, delay));
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    public void StopTimer() => timerActive = false;
    public void AddTime(float seconds) => currentTime += seconds;
    public float GetTimeRemaining() => currentTime;
}