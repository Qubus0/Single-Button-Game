using System;
using System.Collections.Generic;
using Prototype;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(fileName = "BulletPattern", menuName = "Bullet Pattern", order = 0)]
    public class BulletPattern : ScriptableObject
    {
        public int patternDifficulty = 1;
        private int targetDifficulty = 1;

        [SerializeField] public List<Bullet> bulletTypes = new();
        [SerializeField] public List<bool> pattern = new();

        public Bullet BulletPrefab { get; private set; }

        public BulletTelegraphLine TelegraphLine => BulletPrefab.telegraphLine;
        public float TelegraphTime => BulletPrefab.telegraphTime;

        private void Awake()
        {
            if (bulletTypes.Count == 0)
                throw new Exception("Bullet pattern has no bullets");
        }

        public void SetTargetDifficulty(int newDifficulty)
        {
            // Set the initial bullet type based on the target difficulty
            targetDifficulty = newDifficulty;
            BulletPrefab = SelectBulletType(targetDifficulty - patternDifficulty);
        }

        private Bullet SelectBulletType(int remainingDifficulty)
        {
            // Filter out all bullet types that are too difficult
            List<Bullet> validBullets = bulletTypes.FindAll(bullet => bullet.Difficulty <= remainingDifficulty);

            // If no appropriate bullet type is found, use the first one
            if (validBullets.Count == 0)
                return bulletTypes[0];
            // select a random bullet type from the remaining ones
            return validBullets[UnityEngine.Random.Range(0, validBullets.Count)];
        }
    }
}