using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 3f); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
