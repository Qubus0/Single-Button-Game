using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerMove;
    public static event Action OnClockHandMove;
    
        
    public static void PlayerMoved()
    {
        OnPlayerMove?.Invoke();
    }
    
    public static void ClockHandMoved()
    {
        OnClockHandMove?.Invoke();
    }
}