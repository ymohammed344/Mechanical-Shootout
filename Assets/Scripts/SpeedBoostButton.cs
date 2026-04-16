using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpeedBoostButton : MonoBehaviour
{
    public Button boostButton;
    public float boostDuration = 30f;
    public float boostMultiplier = 2f;
    public int boostCost = 10;

    public TextMeshProUGUI feedbackText;
    public float messageDelay = 2f;

    void Start()
    {
        boostButton.onClick.AddListener(ActivateSpeedBoost);
    }

    void ActivateSpeedBoost()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (currentCoins >= boostCost)
        {
            CoinCollector.SpendCoinsStatic(boostCost);

            PlayerPrefs.SetInt("SpeedBoostActive", 1);
            PlayerPrefs.SetFloat("BoostDuration", boostDuration);
            PlayerPrefs.SetFloat("BoostMultiplier", boostMultiplier);
            PlayerPrefs.Save();

            feedbackText.text = "Speed boost activated!";
            StartCoroutine(LoadSceneAfterDelay());
        }
        else
        {
            feedbackText.text = "Not enough coins! You need 10";
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(messageDelay);
        SceneManager.LoadScene(1);
    }
}
