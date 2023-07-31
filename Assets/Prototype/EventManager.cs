using System;
using UnityEngine;

namespace Prototype
{
    public class EventManager : MonoBehaviour
    {
        public static event Action OnPlayerMove;
        
        public static void PlayerMoved()
        {
            OnPlayerMove?.Invoke();
        }
    }
}
