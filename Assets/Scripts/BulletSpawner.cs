using System.Collections.Generic;
using Assets;
using UnityEditor;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Player target;

    [SerializeField] private float delayBetweenShots = 1;

    [SerializeField] private List<BulletPattern> bulletPatterns = new();

    private const float AngleStep = 360f / 60f;


    private BulletPattern currentPattern; // todo use coroutine instead of invoke
    private BulletPattern lastPattern;
    [SerializeField] private int difficulty = 1;

    private void Awake()
    {
        Player.OnPlayerMove += OnPlayerMoved;
        Player.OnPlayerMoveBackwards += OnPlayerMoved;
    }

    private void Start()
    {
        // if there are not bullet patterns set, get them from Assets/Assets/BulletPatternAssets
        LoadBulletPatterns();
        if (bulletPatterns.Count == 0)
            Debug.LogError("No bullet patterns found");

        InvokeRepeating(nameof(QueuePattern), 0, delayBetweenShots);
    }

    private void LoadBulletPatterns()
    {
        // Clear the list to avoid duplicates when reloading in the Editor
        bulletPatterns.Clear();

        string folderPath = "Assets/Assets/BulletPatternAssets";

        // Load all GameObject assets in the specified folder
        string[] guids = AssetDatabase.FindAssets("t:BulletPattern", new[] { folderPath });

        // Add loaded GameObject assets to the bulletPatterns list
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            BulletPattern pattern = AssetDatabase.LoadAssetAtPath<BulletPattern>(assetPath);
            if (pattern != null)
            {
                bulletPatterns.Add(pattern);
            }
        }
    }


    public void IncreaseDifficulty() => difficulty++;

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
        targetAngle += currentPattern.patternStartingIndex * AngleStep;

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

            targetAngle += AngleStep;
        }

        // reset rotation
    }

    private void ShootPattern()
    {
        float targetAngle = target.transform.rotation.eulerAngles.y;
        targetAngle += currentPattern.patternStartingIndex * AngleStep;

        foreach (bool shouldShoot in currentPattern.pattern)
        {
            if (shouldShoot)
                ShootBullet(targetAngle);
            // rotate 1/60th of a circle for every shot
            targetAngle += AngleStep;
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