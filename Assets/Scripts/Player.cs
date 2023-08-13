using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerMove;
    public static event Action OnPlayerMoveBackwards;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spritesSide;
    [SerializeField] private Sprite[] spritesBack;
    [SerializeField] private Sprite[] spritesFront;
    private int spriteIndex;
    private Camera cam;
    
    private readonly int damage = 10;
    private bool directionForward = true;
    
    private bool spacePressed;
    private float spaceHeldTime;
    [SerializeField] private float holdThreshold = 0.2f;

    private void Start()
    {
        cam = Camera.main;
        if (cam == null) throw new Exception("No main camera found");
    }

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
                MoveForward();

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
    
    private void MoveForward()
    {
        gameObject.transform.Rotate(0,  (360f / 60f), 0);
        OnPlayerMove?.Invoke();
        
        directionForward = true;
        spriteIndex++;
        if (spriteIndex > 4)
            spriteIndex = 1;
        AdjustSprite();
    }
    
    private void MoveBackwards()
    {
        gameObject.transform.Rotate(0, -(360f / 60f), 0);
        OnPlayerMoveBackwards?.Invoke();

        directionForward = false;
        spriteIndex++;
        if (spriteIndex > 4)
            spriteIndex = 1;
        AdjustSprite();
    }
    
    private void AdjustSprite()
    {
        // sprite direction
        float angle = gameObject.transform.rotation.eulerAngles.y;
        Sprite sprite;
        bool flip = false;

        bool direction = directionForward;
        // bottom and left of the circle, invert the flip logic
        if (angle is > 135 and < 315)
            direction = !directionForward;

        if (angle is > 45 and < 135 or > 225 and < 315)
            sprite = direction ? spritesFront[spriteIndex] : spritesBack[spriteIndex];
        else
        {
            // top and bottom of the circle, use side sprites
            sprite = spritesSide[spriteIndex];
            flip = !direction;
        }

        spriteRenderer.sprite = sprite;
        spriteRenderer.flipX = flip;

        // face camera
        spriteRenderer.transform.rotation = cam.transform.rotation;
    }
}