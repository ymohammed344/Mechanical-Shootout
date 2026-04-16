using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuyableButton : MonoBehaviour
{
    [Header("Assign the specific button for this mode")]
    public Button targetButton;

    [Header("Set the cost and the scene to load")]
    public int price = 5;
    public int sceneToLoad = 8;

    [Header("Optional: Show coin count")]
    public TextMeshProUGUI coinTextDisplay;

    [Header("Feedback Message")]
    public TextMeshProUGUI feedbackText;
    public float messageDuration = 4f;

    [Header("Optional: Lock overlay or price display")]
    public GameObject lockIcon;

    private string unlockKey;

    void Start()
    {
        unlockKey = "Unlocked_" + sceneToLoad;

        if (targetButton != null)
        {
            targetButton.onClick.AddListener(TryBuyAndPlay);
            CheckUnlockStatus();
        }
    }

    void TryBuyAndPlay()
    {
        if (IsUnlocked())
        {
            LoadScene();
            return;
        }

        int currentCoins = PlayerPrefs.GetInt("Coins", 0);

        if (currentCoins >= price)
        {
           
            // Deduct coins and unlock
            int newTotal = currentCoins - price;
            PlayerPrefs.SetInt("Coins", newTotal);
            PlayerPrefs.SetInt(unlockKey, 1); 
            PlayerPrefs.SetInt(unlockKey, 1); // Mark as unlocked
            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();

            if (coinTextDisplay != null)
                coinTextDisplay.text = "You have " + newTotal + (newTotal == 1 ? " coin" : " coins");

            UpdateVisualsAsUnlocked();
            LoadScene();
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Not enough coins! You need " + price;
                StartCoroutine(ClearMessageAfterDelay());
            }
        }
    }

    IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        if (feedbackText != null)
            feedbackText.text = "";
    }

    void CheckUnlockStatus()
    {
        if (IsUnlocked())
        {
            UpdateVisualsAsUnlocked();
        }

       
        if (coinTextDisplay != null)
        {
            int currentCoins = PlayerPrefs.GetInt("Coins", 0);
            coinTextDisplay.text = "You have " + currentCoins +
                (currentCoins == 1 ? " coin" : " coins");
        }
    }

    void UpdateVisualsAsUnlocked()
    {

        targetButton.interactable = true;

        // Unlock the button fully
        targetButton.interactable = true;

        // Hide lock icon if assigned
        if (lockIcon != null)
            lockIcon.SetActive(false);

  
        // Optional: Clear price display
        if (coinTextDisplay != null)
            coinTextDisplay.text = "Unlocked";
    }

    bool IsUnlocked()
    {
        return PlayerPrefs.GetInt(unlockKey, 0) == 1;
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnEnable()
    {
        CheckUnlockStatus();
    }
}