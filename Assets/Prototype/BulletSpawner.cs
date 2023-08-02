using System.Collections.Generic;
using Assets;
using UnityEngine;

namespace Prototype
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Player target;

        [SerializeField] private float delayBetweenShots = 1;

        [SerializeField] private List<BulletPattern> bulletPatterns = new();

        private BulletPattern currentPattern; // todo use coroutine instead of invoke
        private BulletPattern lastPattern;
        [SerializeField] private int difficulty = 2;
        
        private void Awake()
        {
            EventManager.OnPlayerMove += OnPlayerMoved;
        }

        private void Start()
        {
            InvokeRepeating(nameof(QueuePattern), 0, delayBetweenShots);
        }

        private void OnPlayerMoved()
        {
            gameObject.transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
        }

        private void QueuePattern()
        {
            // filter out all patterns that are too difficult
            List<BulletPattern> validPatterns = bulletPatterns.FindAll(pattern => pattern.patternDifficulty <= difficulty);
            
            // select a random bullet pattern
            currentPattern = validPatterns[Random.Range(0, validPatterns.Count)];
            // if the pattern is the same as the last one, re roll once
            if (currentPattern == lastPattern)
                currentPattern = validPatterns[Random.Range(0, validPatterns.Count)];
            lastPattern = currentPattern;
            currentPattern.SetTargetDifficulty(difficulty);
            
            TelegraphPattern();
            Invoke(nameof(ShootPattern), currentPattern.TelegraphTime);
        }

        private void TelegraphPattern()
        {
            float targetAngle = 0f;
            foreach (bool shot in currentPattern.pattern)
            {
                if (shot)
                {
                    var lineObject = Instantiate(currentPattern.TelegraphLine, gameObject.transform, false);
                    lineObject.MaxLifetime = currentPattern.TelegraphTime;
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
            float targetAngle = target.transform.rotation.eulerAngles.y;
            foreach (bool shouldShoot in currentPattern.pattern)
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
            var bulletPrefab = currentPattern.BulletPrefab;

            var bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
            bullet.transform.Rotate(0, angle, 0);
        }
    }
}