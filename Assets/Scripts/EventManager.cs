using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnPlayerMove;
        
    public static void PlayerMoved()
    {
        OnPlayerMove?.Invoke();
    }
}