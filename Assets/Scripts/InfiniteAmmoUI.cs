using UnityEngine;
using UnityEngine.UI;

public class InfiniteAmmoUI : MonoBehaviour
{
    [Header("UI References")]
    public Text infiniteAmmoText;

    void Update()
    {
        if (infiniteAmmoText == null) return;

        bool hasInfiniteAmmo = InfiniteAmmoButton.HasInfiniteAmmo();

        if (hasInfiniteAmmo)
        {
            float timeLeft = InfiniteAmmoButton.GetTimeRemaining();
            int seconds = Mathf.CeilToInt(timeLeft);
            infiniteAmmoText.text = $"∞ AMMO: {seconds}s";
            infiniteAmmoText.gameObject.SetActive(true);
        }
        else
        {
            infiniteAmmoText.gameObject.SetActive(false);
        }
    }
}
