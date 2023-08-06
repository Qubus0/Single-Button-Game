using System;
using UnityEngine;

namespace Assets
{
    public class Obstacle : MonoBehaviour
    {
        public int difficulty = 1;
        
        public static event Action OnObstacleDestroyed;
        
        [SerializeField] private float moveFrequency = 0;
        [SerializeField] private int damage = 1;
        
        private void OnCollisionEnter(Collision collision)
        {
            DealDamage(collision.gameObject);
        }

        private void DealDamage(GameObject otherGameObject)
        {
            if (otherGameObject.TryGetComponent(out Health health))
                health.TakeDamage(damage);
        }

        private void OnDestroy()
        {
            OnObstacleDestroyed?.Invoke();
        }
    }
}