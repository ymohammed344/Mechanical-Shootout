using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class InfiniteAmmoButton : MonoBehaviour
{
    public Button buyButton;
    public TextMeshProUGUI feedbackText;
    public float messageDelay = 2f;
    public int ammoCost = 15;
    public float ammoDuration = 30f;

    private bool ammoActiveNow = false;

    void Start()
    {
        ammoActiveNow = HasInfiniteAmmo();
        buyButton.onClick.AddListener(BuyInfiniteAmmo);
    }

    void BuyInfiniteAmmo()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (ammoActiveNow)
        {
            feedbackText.text = "Infinite ammo is already active!";
            return;
        }

        if (currentCoins >= ammoCost)
        {
            CoinCollector.SpendCoinsStatic(ammoCost);

            ammoActiveNow = true;
            
            float endTime = Time.time + ammoDuration;
            PlayerPrefs.SetFloat("InfiniteAmmoEndTime", endTime);
            PlayerPrefs.SetInt("InfiniteAmmoActive", 1);
            PlayerPrefs.Save();

            feedbackText.text = "Infinite Ammo Activated!";

            StartCoroutine(LoadSceneAfterDelay());
        }
        else
        {
            feedbackText.text = $"Not enough coins! You need {ammoCost}";
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(messageDelay);
        SceneManager.LoadScene(1);
    }

    public static bool HasInfiniteAmmo()
    {
        if (PlayerPrefs.GetInt("InfiniteAmmoActive", 0) == 0)
            return false;

        float endTime = PlayerPrefs.GetFloat("InfiniteAmmoEndTime", 0f);
        
        if (Time.time >= endTime)
        {
            PlayerPrefs.SetInt("InfiniteAmmoActive", 0);
            PlayerPrefs.Save();
            return false;
        }

        return true;
    }

    public static float GetTimeRemaining()
    {
        if (!HasInfiniteAmmo())
            return 0f;

        float endTime = PlayerPrefs.GetFloat("InfiniteAmmoEndTime", 0f);
        return Mathf.Max(0f, endTime - Time.time);
    }
}
