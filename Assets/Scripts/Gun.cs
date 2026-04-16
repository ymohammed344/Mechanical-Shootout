using UnityEngine;
using TMPro;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI outOfAmmoText; 
    public GameObject muzzleFlashPrefab;
    public AudioSource audioSource;
    public AudioClip gunshotClip;

    [Header("Gun Settings")]
    public float bulletForce = 20f;
    public int maxAmmo = 30;
    public int maxReserveAmmo = 90;
    public float reloadTime = 2f;
    public float fireRate = 0.2f;

    private int currentAmmo;
    private int reserveAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    private bool outOfAmmoWarningActive = false;

    private float infiniteAmmoTimeLeft = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        reserveAmmo = maxReserveAmmo;
        if (timerText) timerText.text = "";
        if (outOfAmmoText) outOfAmmoText.gameObject.SetActive(false); 
    }

    void Update()
    {
        if (isReloading) return;

        bool infiniteAmmoActive = InfiniteAmmoButton.HasInfiniteAmmo();

        // Fire
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && (infiniteAmmoActive || currentAmmo > 0))
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot(infiniteAmmoActive);
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && !infiniteAmmoActive)
        {
            StartCoroutine(Reload());
        }

        // Ammo UI
        if (ammoText)
        {
            ammoText.text = isReloading ? "Reloading..." :
                infiniteAmmoActive ? "∞ Ammo" : $"Ammo: {currentAmmo}/{reserveAmmo}";
        }

        // Out of Ammo Warning
        if (!infiniteAmmoActive && currentAmmo == 0 && reserveAmmo == 0 && !outOfAmmoWarningActive)
        {
            StartCoroutine(ShowOutOfAmmoWarning());
        }

        // Infinite Ammo Timer
        if (infiniteAmmoActive)
        {
            infiniteAmmoTimeLeft -= Time.deltaTime;
            if (timerText) timerText.text = $"Infinite Ammo Active!";
        }
        else
        {
            if (timerText) timerText.text = "";
        }
    }

    void Shoot(bool infiniteAmmo)
    {
        if (!infiniteAmmo)
            currentAmmo--;

        var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * bulletForce, ForceMode.Impulse);

        var flash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
        Destroy(flash, 0.1f);

        if (gunshotClip != null && audioSource != null)
            audioSource.PlayOneShot(gunshotClip);
    }

    IEnumerator Reload()
    {
        if (reserveAmmo <= 0 || currentAmmo == maxAmmo)
            yield break;

        isReloading = true;
        if (ammoText) ammoText.text = "Reloading...";
        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = maxAmmo - currentAmmo;
        int bulletsToLoad = Mathf.Min(bulletsNeeded, reserveAmmo);

        currentAmmo += bulletsToLoad;
        reserveAmmo -= bulletsToLoad;

        isReloading = false;
    }

    IEnumerator ShowOutOfAmmoWarning()
    {
        if (outOfAmmoText == null) yield break;

        outOfAmmoWarningActive = true;
        outOfAmmoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        outOfAmmoText.gameObject.SetActive(false);
        outOfAmmoWarningActive = false;
    }

    public void RefillAllAmmo()
    {
        currentAmmo = maxAmmo;
        reserveAmmo = maxReserveAmmo;
    }
}
