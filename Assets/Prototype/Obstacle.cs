using UnityEngine;

namespace Prototype
{
    public class Obstacle : MonoBehaviour
    {
        private float damage = 1;
        
        private void OnCollisionEnter(Collision collision)
        {
            DealDamage(collision.gameObject);
            Destroy(gameObject);
        }

        private void DealDamage(GameObject otherGameObject)
        {
            // if (otherGameObject.TryGetComponent(out Health health))
            //     health.TakeDamage(damage);
        }
    }
}