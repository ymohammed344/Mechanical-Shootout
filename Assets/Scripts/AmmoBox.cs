using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class AmmoBox : MonoBehaviour
{
    [Header("Ammo Settings")]
    [Tooltip("Amount of reserve ammo to add")]
    public int ammoAmount = 30;

    [Header("Audio")]
    public AudioClip pickupSound;
    public float soundVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmmoManager ammoManager = other.GetComponent<AmmoManager>();
            if (ammoManager != null)
            {
                ammoManager.AddReserveAmmo(ammoAmount);
                Debug.Log($"Picked up {ammoAmount} bullets!");
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }

            Destroy(gameObject);
        }
    }
}
