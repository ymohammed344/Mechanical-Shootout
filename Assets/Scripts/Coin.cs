using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Pickup Sound")]
    public AudioClip pickupSound;   
    public float soundVolume = 1f;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }

      
            Destroy(gameObject);
        }
    }
}

