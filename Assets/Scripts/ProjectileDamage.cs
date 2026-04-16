using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage dealt to enemies (percentage: 0.05 = 5%)")]
    public float damagePercent = 0.05f;

    [Tooltip("Damage dealt to player")]
    public float playerDamage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy4"))
        {
            EnemyHealthUI enemy = collision.gameObject.GetComponent<EnemyHealthUI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePercent);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (ShieldManager.Instance != null && ShieldManager.Instance.IsShieldActive())
            {
                return;
            }

            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(playerDamage);
            }
        }
    }
}

