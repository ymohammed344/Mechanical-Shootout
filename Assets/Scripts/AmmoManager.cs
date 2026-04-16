using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class AmmoManager : MonoBehaviour
{
    [Header("Assault Rifle Ammo Settings")]
    public int assaultRifleMaxReserve = 90;
    public int assaultRifleStartingReserve = 60;

    [Header("Pistol Ammo Settings")]
    public int pistolMaxReserve = 20;
    public int pistolStartingReserve = 20;

    private int assaultRifleReserveAmmo;
    private int pistolReserveAmmo;
    private CharacterBehaviour character;
    private string lastWeaponName = "";

    void Start()
    {
        character = GetComponent<CharacterBehaviour>();
        assaultRifleReserveAmmo = assaultRifleStartingReserve;
        pistolReserveAmmo = pistolStartingReserve;
    }

    void Update()
    {
        if (character == null) return;

        InventoryBehaviour inventory = character.GetInventory();
        if (inventory == null) return;

        WeaponBehaviour weapon = inventory.GetEquipped();
        if (weapon == null)
        {
            lastWeaponName = "";
            return;
        }

        string weaponName = weapon.name;

        if (lastWeaponName != weaponName)
        {
            lastWeaponName = weaponName;
        }
    }

    private bool IsPistol(string weaponName)
    {
        return weaponName.Contains("Handgun") || weaponName.Contains("Pistol");
    }

    private int GetCurrentReserveForWeapon(string weaponName)
    {
        return IsPistol(weaponName) ? pistolReserveAmmo : assaultRifleReserveAmmo;
    }

    private int GetMaxReserveForWeapon(string weaponName)
    {
        return IsPistol(weaponName) ? pistolMaxReserve : assaultRifleMaxReserve;
    }

    public int CalculateReloadAmount(int currentAmmo, int magazineSize)
    {
        if (character == null) return 0;

        InventoryBehaviour inventory = character.GetInventory();
        if (inventory == null) return 0;

        WeaponBehaviour weapon = inventory.GetEquipped();
        if (weapon == null) return 0;

        string weaponName = weapon.name;
        bool isPistol = IsPistol(weaponName);
        int currentReserve = isPistol ? pistolReserveAmmo : assaultRifleReserveAmmo;

        if (currentReserve <= 0)
        {
            Debug.Log("Cannot reload: No reserve ammo remaining!");
            return 0;
        }

        int bulletsNeeded = magazineSize - currentAmmo;
        int bulletsToLoad = Mathf.Min(bulletsNeeded, currentReserve);
        
        if (isPistol)
            pistolReserveAmmo -= bulletsToLoad;
        else
            assaultRifleReserveAmmo -= bulletsToLoad;

        Debug.Log($"Reload: Added {bulletsToLoad} bullets. Reserve left: {(isPistol ? pistolReserveAmmo : assaultRifleReserveAmmo)}");
        
        return bulletsToLoad;
    }

    public void AddReserveAmmo(int amount)
    {
        if (character == null) return;

        InventoryBehaviour inventory = character.GetInventory();
        if (inventory == null) return;

        WeaponBehaviour weapon = inventory.GetEquipped();
        if (weapon == null) return;

        string weaponName = weapon.name;
        bool isPistol = IsPistol(weaponName);
        int maxForWeapon = GetMaxReserveForWeapon(weaponName);

        if (isPistol)
        {
            pistolReserveAmmo = Mathf.Clamp(pistolReserveAmmo + amount, 0, maxForWeapon);
            Debug.Log($"Picked up {amount} ammo. Pistol Reserve: {pistolReserveAmmo}/{maxForWeapon}");
        }
        else
        {
            assaultRifleReserveAmmo = Mathf.Clamp(assaultRifleReserveAmmo + amount, 0, maxForWeapon);
            Debug.Log($"Picked up {amount} ammo. Assault Rifle Reserve: {assaultRifleReserveAmmo}/{maxForWeapon}");
        }
    }

    public int GetCurrentReserveAmmo()
    {
        if (character == null) return 0;

        InventoryBehaviour inventory = character.GetInventory();
        if (inventory == null) return 0;

        WeaponBehaviour weapon = inventory.GetEquipped();
        if (weapon == null) return 0;

        return GetCurrentReserveForWeapon(weapon.name);
    }
    
    public int GetMaxReserveAmmo()
    {
        if (character == null) return assaultRifleMaxReserve;

        InventoryBehaviour inventory = character.GetInventory();
        if (inventory == null) return assaultRifleMaxReserve;

        WeaponBehaviour weapon = inventory.GetEquipped();
        if (weapon == null) return assaultRifleMaxReserve;

        return GetMaxReserveForWeapon(weapon.name);
    }
}
