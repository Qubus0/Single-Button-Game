using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerMove;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.transform.Rotate(0, 360f/60f, 0);
            OnPlayerMove?.Invoke();
        }
    }
}