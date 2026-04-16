using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShieldButton : MonoBehaviour
{
    public Button buyButton;
    public TextMeshProUGUI feedbackText;
    public float messageDelay = 2f;
    public int shieldCost = 10;
    public float shieldDuration = 30f;

    void Start()
    {
        buyButton.onClick.AddListener(UnlockShield);
    }

    void UnlockShield()
    {
        if (PlayerPrefs.GetInt("ShieldActive", 0) == 1)
        {
            feedbackText.text = "Shield is already active!";
            return;
        }

        int totalCoins = PlayerPrefs.GetInt("Coins", 0);
        if (totalCoins < shieldCost)
        {
            feedbackText.text = $"Not enough coins! You need {shieldCost}";
            return;
        }

        CoinCollector.SpendCoinsStatic(shieldCost);

        PlayerPrefs.SetInt("ShieldActive", 1);
        PlayerPrefs.SetFloat("ShieldDuration", shieldDuration);
        PlayerPrefs.Save();

        feedbackText.text = "Shield Activated!";
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(messageDelay);
        SceneManager.LoadScene(1);
    }
}
