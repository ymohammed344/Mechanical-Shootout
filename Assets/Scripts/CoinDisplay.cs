using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinDisplayText;

    void OnEnable()
    {
        CoinCollector.OnCoinsChanged += UpdateDisplay;

        int totalCoins = PlayerPrefs.GetInt("Coins", 0);
        UpdateDisplay(totalCoins);
    }

    void OnDisable()
    {
        CoinCollector.OnCoinsChanged -= UpdateDisplay;
    }

    void UpdateDisplay(int totalCoins)
    {
        if (coinDisplayText != null)
            coinDisplayText.text = " " + totalCoins + (totalCoins == 1 ? " coin" : " coins");
    }
}
