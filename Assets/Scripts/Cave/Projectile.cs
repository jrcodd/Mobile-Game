using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Set the damage value for the projectile
    public string senderTag;
    void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collides with the player or enemy
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            if(other.CompareTag(senderTag))
            {
                return;
            }   
            health.TakeDamage(damage, true);
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                Destroy(gameObject); // Destroy the projectile upon collision
            }
        }
        if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        // Destroy the projectile when it goes off-screen
        Destroy(gameObject);
    }
}
