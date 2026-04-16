using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public float healAmount = 25f;         
    public AudioClip healSound;            
    public float soundVolume = 1f;          

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
         
                if (playerHealth.currentHealth < playerHealth.maxHealth - 0.01f)
                {
                    float oldHealth = playerHealth.currentHealth;

     
                    playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth + healAmount, playerHealth.maxHealth);

         
                    if (playerHealth.healthSlider != null)
                        playerHealth.healthSlider.value = playerHealth.currentHealth;

                 
                    if (playerHealth.currentHealth > oldHealth)
                    {
                        if (healSound != null)
                            AudioSource.PlayClipAtPoint(healSound, transform.position, soundVolume);

                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
