using UnityEngine;
using TMPro;
using InfimaGames.LowPolyShooterPack;

public class AmmoDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI lowAmmoText;

    [Header("Settings")]
    public int lowAmmoThreshold = 10;

    private CharacterBehaviour character;
    private InventoryBehaviour inventory;
    private AmmoManager ammoManager;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            character = player.GetComponent<CharacterBehaviour>();
            ammoManager = player.GetComponent<AmmoManager>();
            
            if (character != null)
            {
                inventory = character.GetInventory();
            }
        }

        if (lowAmmoText != null)
        {
            lowAmmoText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (inventory == null) return;

        WeaponBehaviour equippedWeapon = inventory.GetEquipped();
        
        if (equippedWeapon != null && ammoManager != null)
        {
            int currentAmmo = equippedWeapon.GetAmmunitionCurrent();
            int reserveAmmo = ammoManager.GetCurrentReserveAmmo();

            if (ammoText != null)
            {
                ammoText.text = $"{currentAmmo} / {reserveAmmo}";
            }

            if (lowAmmoText != null)
            {
                if (currentAmmo <= lowAmmoThreshold && currentAmmo > 0)
                {
                    lowAmmoText.gameObject.SetActive(true);
                    lowAmmoText.text = "Low Ammo! Find Ammo Box!";
                }
                else if (currentAmmo == 0 && reserveAmmo == 0)
                {
                    lowAmmoText.gameObject.SetActive(true);
                    lowAmmoText.text = "Out of Ammo! Find Ammo Box!";
                }
                else if (currentAmmo == 0)
                {
                    lowAmmoText.gameObject.SetActive(true);
                    lowAmmoText.text = "Press R to Reload";
                }
                else
                {
                    lowAmmoText.gameObject.SetActive(false);
                }
            }
        }
    }
}
