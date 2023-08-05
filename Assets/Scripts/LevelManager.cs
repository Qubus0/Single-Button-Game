using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public int difficulty = 1;
    
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
        
        InvokeRepeating(nameof(TimeTick), 0, 1);
    }
    
    private void TimeTick()
    {
        OnClockHandMove();
        onTimeTick?.Invoke();
    }

    private void OnPlayerMove() => totalPlayerRotationDegrees += 360f / 60f;
    
    private void OnPlayerMoveBackwards() => totalPlayerRotationDegrees -= 360f / 60f;

    private void OnClockHandMove() => totalTimeHandRotationDegrees += 360f / 60f;

    private void OnMove()
    {
        float distance = totalPlayerRotationDegrees - totalTimeHandRotationDegrees;
        
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
        float newTimeScale = Mathf.Clamp(timeScale + distance / 360f, 1f, 1f + difficultyGrowingSpeedUpMax);
        
        // if the player is behind the clock hand, lower the time scale
        if (distance < 0)
            newTimeScale = timeScale - Mathf.Clamp(-distance / 360f, 0f, 0.5f);
        
        
        Time.timeScale = newTimeScale;
    }
}