using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerMove;
    public static event Action OnPlayerMoveBackwards;
    [SerializeField] private float playerMoveRepeatTime = 0.1f;
    private float playerSpeedMultiplier = 1f;

    private enum State
    {
        None,
        Pressed,
        Held,
    }
    private State state = State.None;
    
    private int damage = 1;
    private int direction = 1;

    private void Start()
    {
        InvokeRepeating(nameof(Move), 0, playerMoveRepeatTime);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerSpeedMultiplier = 1f;
            CancelInvoke(nameof(Move));
            InvokeRepeating(nameof(Move), playerMoveRepeatTime, playerMoveRepeatTime);
            direction *= -1;
            gameObject.transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    // private void Update()
    // {
    //     // Check for space key press
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // Call Move immediately
    //         state = State.Pressed;
    //         // delay a bit to give the player more control   
    //         InvokeRepeating(nameof(Move), holdThreshold, playerMoveRepeatTime);
    //     }
    //
    //     // Check for space key release
    //     if (Input.GetKeyUp(KeyCode.Space))
    //     {
    //         if (state == State.Pressed)
    //         {
    //             CancelInvoke(nameof(Move));
    //             MoveBackwards();
    //         }
    //         else if (state == State.Held)
    //             CancelInvoke(nameof(Move));
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }
    
    private void DealDamage(GameObject otherGameObject)
    {
        if (otherGameObject.TryGetComponent(out Health health))
            health.TakeDamage(damage);
    }
    
    private void Move()
    {
        playerSpeedMultiplier -= 0.05f;
        playerSpeedMultiplier = Mathf.Clamp(playerSpeedMultiplier, 0.3f, 1f);
        
        gameObject.transform.Rotate(0, direction * (360f / 60f), 0);
        OnPlayerMove?.Invoke();
    }

    private void MoveBackwards()
    {
        gameObject.transform.Rotate(0, -360f / 60f, 0);
        OnPlayerMoveBackwards?.Invoke();
    }
}