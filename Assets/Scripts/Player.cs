using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerMove;
    [SerializeField] private float playerSpeed = 0.1f;

    
    private void Update()
    {
        // Check for space key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call Move immediately
            Move();
            // delay a bit to give the player more control   
            InvokeRepeating(nameof(Move), playerSpeed * 2, playerSpeed);
        }

        // Check for space key release
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke(nameof(Move));
        }
    }

    private void Move()
    {
        gameObject.transform.Rotate(0, 360f / 60f, 0);
        OnPlayerMove?.Invoke();
    }
}