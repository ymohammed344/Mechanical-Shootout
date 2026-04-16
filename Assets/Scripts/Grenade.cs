using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public float playerDamage = 50f;

    public GameObject gun1;
    public GameObject gun2;
    public GameObject hand1;
    public GameObject hand2;

    public GameObject explosionEffect;

    [Header("Audio")]
    public AudioClip explosionSound;
    private AudioSource audioSource;

    private float countdown;
    private bool hasExploded = false;

    void Start()
    {
        countdown = delay;

      
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; 
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
            hasExploded = true;
        }
        else if (collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Wall"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            transform.SetParent(collision.transform);
        }
    }

    void Explode()
    {
  
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, 2f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            if (nearbyObject.CompareTag("Enemy"))
            {
                EnemyHealthUI enemyHealth = nearbyObject.GetComponent<EnemyHealthUI>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1.0f);
                }
                else
                {
                    Destroy(nearbyObject.gameObject);
                }
            }

            if (nearbyObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(playerDamage);
                }
            }
        }

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        if (gun1 != null) gun1.SetActive(true);
        if (gun2 != null) gun2.SetActive(true);
        if (hand1 != null) hand1.SetActive(true);
        if (hand2 != null) hand2.SetActive(true);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
