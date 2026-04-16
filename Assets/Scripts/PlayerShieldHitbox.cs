using UnityEngine;

public class PlayerShieldHitbox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!ShieldManager.Instance || !ShieldManager.Instance.IsShieldActive())
            return;


        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
            Debug.Log("Bullet blocked by shield");
        }
    }

  
    public bool TryBlockDamage()
    {
        return ShieldManager.Instance != null && ShieldManager.Instance.IsShieldActive();
    }
}
