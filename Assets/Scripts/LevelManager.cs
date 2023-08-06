using System;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public int difficulty = 1;
    
    [SerializeField] private GameObject healthClockHand; 
    private ClockHand clockHandScript;
    
    [SerializeField] private GameObject player;
    private Health playerHealth;

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
        
        clockHandScript = healthClockHand.GetComponent<ClockHand>();

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
        // if (health <= 0)
        //     Time.timeScale = 0f;
    }

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