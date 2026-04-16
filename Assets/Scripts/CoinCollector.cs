using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    [Header("Pickup Sound")]
    public AudioClip pickupSound;
    public float soundVolume = 1f;

    public static event System.Action<int> OnCoinsChanged;

    void Start()
    {
        coinCount = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            int randomAmount = GetRandomCoinValue();
            coinCount += randomAmount;

            PlayerPrefs.SetInt("Coins", coinCount);
            PlayerPrefs.Save();

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, other.transform.position, soundVolume);

            UpdateCoinUI();
            Destroy(other.gameObject);
        }
    }

    int GetRandomCoinValue()
    {
        int[] possibleValues = { 1, 2, 5 };
        return possibleValues[Random.Range(0, possibleValues.Length)];
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = " " + coinCount + (coinCount == 1 ? " coin" : " coins");

        OnCoinsChanged?.Invoke(coinCount);
    }

    public static void SpendCoinsStatic(int amount)
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        coins -= amount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(coins);
    }
}
