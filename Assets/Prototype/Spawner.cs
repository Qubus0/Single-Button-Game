using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject target;
        [SerializeField] private float bulletSpeed = 1;
        [SerializeField] private float delaySeconds = 1;

        [SerializeField] private List<bool> bulletPattern = new();

        private void Start()
        {
            InvokeRepeating(nameof(Shoot), 0, delaySeconds);
        }
    
        private void Shoot()    
        {
            List<bool> currentPattern = bulletPattern;
            float targetAngle = target.transform.rotation.eulerAngles.y;
            // turn towards the target
            gameObject.transform.Rotate(0, targetAngle, 0);
        
            // patternBeginning = center

            foreach (bool shouldShoot in currentPattern)
            {
                if (shouldShoot)
                    SpawnBullet();
                // rotate 1/60th of a circle for every shot
                gameObject.transform.Rotate(0, 360/60, 0);
            }
            // reset rotation
            gameObject.transform.rotation = Quaternion.identity;
        }

        // Update is called once per frame
        private void SpawnBullet()
        {
            var o = gameObject;
            var bullet = Instantiate(bulletPrefab, o.transform.position, o.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * bulletSpeed;
        }
    }
}
