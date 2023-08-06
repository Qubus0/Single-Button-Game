using System.Collections;
using Assets;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public int difficulty = 1;
    public int score = 0;

    [SerializeField] private GameObject healthClockHand;
    private ClockHand clockHandScript;

    [SerializeField] private GameObject player;
    private Health playerHealth;

    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject scoreText;

    [SerializeField] private ArcDraw arcDraw;

    [SerializeField] private float difficultyPermanentSpeedUp = 0.1f;
    [SerializeField] private float difficultyGrowingSpeedUpMax = 0.5f;
    private float timeScale = 1f;

    private float totalPlayerRotationDegrees = 0f;
    private float totalTimeHandRotationDegrees = 0f;

    public UnityEvent onTimeTick;
    public UnityEvent onDifficultyIncrease;

    private void Awake()
    {
        Player.OnPlayerMove += OnPlayerMove;
        Player.OnPlayerMoveBackwards += OnPlayerMoveBackwards;
        Player.OnPlayerMove += OnMove;
        Player.OnPlayerMoveBackwards += OnMove;

        playerHealth = player.GetComponent<Health>();
        playerHealth.OnHealthChanged += OnPlayerHealthChanged;

        Obstacle.OnObstacleDestroyed += OnObstacleDestroyed;

        clockHandScript = healthClockHand.GetComponent<ClockHand>();

        UpdateScoreText();
        InvokeRepeating(nameof(TimeTick), 0, 1);
    }

    private void TimeTick()
    {
        OnClockHandMove();
        OnMove();
        onTimeTick?.Invoke();
    }

    private void OnObstacleDestroyed()
    {
        score += difficulty;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.GetComponent<TextMeshPro>().text = score.ToString();
    }

    private void OnPlayerMove() => totalPlayerRotationDegrees += 360f / 60f;

    private void OnPlayerMoveBackwards() => totalPlayerRotationDegrees -= 360f / 60f;

    private void OnClockHandMove() => totalTimeHandRotationDegrees += 360f / 60f;

    private void OnPlayerHealthChanged(int health)
    {
        // update ui
        float shownHealth = clockHandScript.GetCurrentTick();
        float diff = shownHealth - health;

        while (diff != 0)
        {
            shownHealth = clockHandScript.GetCurrentTick();
            diff = shownHealth - health;

            if (diff > 0)
                clockHandScript.TickBackward();
            else if (diff < 0)
                clockHandScript.TickForward();
        }

        // restart menu
        if (health <= 0)
        {
            StartCoroutine(LerpTimeScaleToZero());

            // Restart();
            // ui.SetActive(true);
        }
    }

    private IEnumerator LerpTimeScaleToZero()
    {
        const float deathLerpDuration = 3f;
        // Store the initial time scale to use as the starting value
        float initialTimeScale = Time.timeScale;

        // The current time in the lerp process
        float currentTime = 0f;

        while (currentTime < deathLerpDuration)
        {
            currentTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore Time.timeScale
            float t = Mathf.Clamp01(currentTime / deathLerpDuration);

            // Lerp the time scale from its initial value to 0
            Time.timeScale = Mathf.Lerp(initialTimeScale, 0f, t);

            yield return null;
        }

        // Ensure that the time scale is exactly 0 when the coroutine finishes
        Time.timeScale = 0f;
    }

    private void OnMove()
    {
        float distance = totalPlayerRotationDegrees - totalTimeHandRotationDegrees;

        if (distance < 0)
            arcDraw.Draw(totalPlayerRotationDegrees, totalTimeHandRotationDegrees);
        else
            arcDraw.Draw(totalTimeHandRotationDegrees, totalPlayerRotationDegrees);

        // if the player overtakes the clock hand, increase the difficulty and reset the counters
        if (distance > 360f)
        {
            onDifficultyIncrease?.Invoke();
            difficulty++;
            timeScale += difficultyPermanentSpeedUp;
            totalPlayerRotationDegrees = 0f;
            totalTimeHandRotationDegrees = 0f;
        }

        // the further the player is from the clock hand, the faster the time scale
        float newTimeScale = Mathf.Clamp(timeScale + distance / 360f, 1f, 0.5f + difficultyGrowingSpeedUpMax);

        // if the player is behind the clock hand, lower the time scale
        if (distance < 0)
            newTimeScale = timeScale - Mathf.Clamp(-distance / 180f, 0f, 0.7f);


        Time.timeScale = newTimeScale;
    }

    public void Restart()
    {
        // Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}