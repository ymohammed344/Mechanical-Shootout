using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class BomberButton : MonoBehaviour
{
    public Button buyButton;
    public TextMeshProUGUI feedbackText;  
    public float messageDelay = 2f;
    public int bomberCost = 15;

    void Start()
    {
        buyButton.onClick.AddListener(BuyGrenades);
    }

    void BuyGrenades()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (currentCoins >= bomberCost)
        {
            CoinCollector.SpendCoinsStatic(bomberCost);

            PlayerPrefs.SetInt("GrenadesPurchased", 1);
            PlayerPrefs.Save();

            feedbackText.text = "2 Grenades Purchased!";
            StartCoroutine(LoadSceneAfterDelay());
        }
        else
        {
            feedbackText.text = "Not enough coins! You need 15";
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(messageDelay);
        SceneManager.LoadScene(1);
    }
}