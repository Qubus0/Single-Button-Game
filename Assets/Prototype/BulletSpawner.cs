using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private BulletTelegraphLine bulletTelegraphingLine;
        [SerializeField] private Player target;

        [SerializeField] private float bulletSpeed = 1;
        [SerializeField] private float delayBetweenShots = 1;
        [SerializeField] private float timeUntilShot = 1;

        [SerializeField] private List<bool> bulletPattern = new();

        private void Start()
        {
            InvokeRepeating(nameof(QueuePattern), 0, delayBetweenShots);
        }

        private void Update()
        {
            gameObject.transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
        }

        private void QueuePattern()
        {
            List<bool> pattern = bulletPattern;
            TelegraphPattern();
            Invoke(nameof(ShootPattern), timeUntilShot);
        }

        private void TelegraphPattern()
        {
            List<bool> pattern = bulletPattern; // todo use coroutine instead of invoke

            float targetAngle = 0f;
            foreach (bool shot in pattern)
            {
                if (shot)
                {
                    var lineObject = Instantiate(bulletTelegraphingLine, gameObject.transform, false);
                    lineObject.MaxLifetime = timeUntilShot;
                    var line = lineObject.GetComponent<LineRenderer>();
                    var lineDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                    line.SetPosition(1, lineDirection * 6);
                }

                targetAngle += 360f / 60f;
            }

            // reset rotation
        }

        private void ShootPattern()
        {
            List<bool> pattern = bulletPattern; // todo use coroutine instead of invoke

            float targetAngle = target.transform.rotation.eulerAngles.y;
            foreach (bool shouldShoot in pattern)
            {
                if (shouldShoot)
                    ShootBullet(targetAngle);
                // rotate 1/60th of a circle for every shot
                targetAngle += 360f / 60f;
            }
        }

        // Update is called once per frame
        private void ShootBullet(float angle)
        {
            var bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
            bullet.transform.Rotate(0, angle, 0);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        }
    }
}