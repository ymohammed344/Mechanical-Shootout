using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostUI : MonoBehaviour
{
    public Text timerText;
    private SpeedBoostManager speedBoostManager;

    void Start()
    {
        speedBoostManager = FindObjectOfType<SpeedBoostManager>();
    }

    void Update()
    {
        if (speedBoostManager != null && speedBoostManager.IsBoostActive())
        {
            float timeLeft = speedBoostManager.GetRemainingTime();
            timerText.text = $"Speed Boost: {timeLeft:F1}s";
            timerText.gameObject.SetActive(true);
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }
}
