using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public int difficulty = 1;
    
    private float timeScale = 1f;
    
    private float totalPlayerRotationDegrees = 0f;
    private float totalTimeHandRotationDegrees = 0f;
    
    public UnityEvent onTimeTick;
    [SerializeField] private ClockHand secondHand;
    
    private void Awake()
    {
        Player.OnPlayerMove += OnPlayerMove;
        Player.OnPlayerMove += OnMove;
        
        InvokeRepeating(nameof(TimeTick), 0, 1);
    }
    
    private void TimeTick() => onTimeTick?.Invoke();
    
    private void OnPlayerMove() => totalPlayerRotationDegrees += 360f / 60f;

    private void OnClockHandMove() => totalTimeHandRotationDegrees += 360f / 60f;

    private void OnMove()
    {
        float distance = totalPlayerRotationDegrees - totalTimeHandRotationDegrees;
        
        // if the player overtakes the clock hand, increase the difficulty and reset the counters
        if (distance > 360f)
        {
            difficulty++;
            totalPlayerRotationDegrees = 0f;
            totalTimeHandRotationDegrees = 0f;
        }


        // the further the player is from the clock hand, the faster the time scale
        float newTimeScale = Mathf.Clamp(1f + distance / 360f, 1f, 2f);
        
        // if the player is behind the clock hand, lower the time scale
        if (distance < 0)
            newTimeScale = 1f - Mathf.Clamp(-distance / 360f, 0f, 0.5f);
        
        timeScale = newTimeScale;
        Time.timeScale = newTimeScale;
    }
}