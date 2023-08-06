using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerMove;
    public static event Action OnPlayerMoveBackwards;
    [SerializeField] private float playerSpeed = 0.1f;

    private bool spacePressed = false;
    private float spaceHeldTime = 0f;
    [SerializeField] private float holdThreshold = 0.3f;
    
    private int damage = 1;

    private void Update()
    {
        // Check if the Space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
            spacePressed = true;

        // Check if the Space key is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (spaceHeldTime >= holdThreshold)
                MoveBackwards();
            else if (spacePressed)
                Move();

            spacePressed = false;
            spaceHeldTime = 0f;
        }

        if (spaceHeldTime >= holdThreshold)
        {
            MoveBackwards();
            spaceHeldTime = 0f;
            spacePressed = false;
        }

        // Increment time if the Space key is held down
        if (spacePressed)
            spaceHeldTime += Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }
    
    private void DealDamage(GameObject otherGameObject)
    {
        if (otherGameObject.TryGetComponent(out Health health))
            health.TakeDamage(damage);
    }

    // private void Update()
    // {
    //     // Check for space key press
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // Call Move immediately
    //         Move();
    //         // delay a bit to give the player more control   
    //         InvokeRepeating(nameof(Move), playerSpeed * 2, playerSpeed);
    //     }
    //
    //     // Check for space key release
    //     if (Input.GetKeyUp(KeyCode.Space))
    //     {
    //         CancelInvoke(nameof(Move));
    //     }
    // }
    //
    private void Move()
    {
        gameObject.transform.Rotate(0, 360f / 60f, 0);
        OnPlayerMove?.Invoke();
    }

    private void MoveBackwards()
    {
        gameObject.transform.Rotate(0, -360f / 60f, 0);
        OnPlayerMoveBackwards?.Invoke();
    }
}