using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float damageAmount = 10f;
    public float destroyAfterSeconds = 5f;
    public float bulletSpeed = 20f;

    private Transform player;

    void Start()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
            Vector3 direction = (player.position - transform.position).normalized;
            var rb = GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = direction * bulletSpeed;
        }

        Destroy(gameObject, destroyAfterSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If shield is active globally — block and destroy projectile
            if (ShieldManager.Instance != null && ShieldManager.Instance.IsShieldActive())
            {
                Destroy(gameObject);
                return;
            }

            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
