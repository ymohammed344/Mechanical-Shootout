using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Button resetButton;

    void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetGameData);
    }

    void ResetGameData()
    {
        // Reset Coins
        PlayerPrefs.DeleteKey("Coins");

        // Reset Unlocked Items
        PlayerPrefs.DeleteKey("Unlocked_9");
        PlayerPrefs.DeleteKey("Unlocked_10");
        PlayerPrefs.DeleteKey("Unlocked_11");

        // Reset Shield Purchase
        PlayerPrefs.DeleteKey("ShieldActive");
        PlayerPrefs.DeleteKey("ShieldDuration");

        // Reset Infinite Ammo Purchase
        PlayerPrefs.DeleteKey("InfiniteAmmoActive");
        PlayerPrefs.DeleteKey("InfiniteAmmoEndTime");

        // Reset Grenade/Bomber Purchase
        PlayerPrefs.DeleteKey("GrenadesPurchased");

        // Reset Speed Boost Purchase
        PlayerPrefs.DeleteKey("SpeedBoostActive");
        PlayerPrefs.DeleteKey("BoostDuration");
        PlayerPrefs.DeleteKey("BoostMultiplier");

        // Reset Sound Settings
        if (AudioManager.instance != null)
            AudioManager.instance.ResetSoundSettings();

        // Save all deletions
        PlayerPrefs.Save();

        Debug.Log("Game data reset! All coins, purchases, unlocks, and sound settings have been cleared.");
    }
}