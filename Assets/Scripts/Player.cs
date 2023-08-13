using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerMove;
    public static event Action OnPlayerMoveBackwards;
    [SerializeField] private float playerMoveRepeatTime = 0.1f;
    private float playerSpeedMultiplier = 1f;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spritesSide;
    [SerializeField] private Sprite[] spritesBack;
    [SerializeField] private Sprite[] spritesFront;
    private int spriteIndex = 0;
    private Camera cam;
    
    private readonly int damage = 10;
    private bool directionForward = true;

    private void Start()
    {
        cam = Camera.main;
        if (cam == null) throw new Exception("No main camera found");
            
        InvokeRepeating(nameof(Move), 0, playerMoveRepeatTime);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spriteIndex = 0;
            playerSpeedMultiplier = 1f;
            CancelInvoke(nameof(Move));
            InvokeRepeating(nameof(Move), playerMoveRepeatTime, playerMoveRepeatTime);
            directionForward = !directionForward;
            AdjustSprite();
        }
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
    
    private void Move()
    {
        spriteIndex++;
        if (spriteIndex > 4)
            spriteIndex = 1;

        playerSpeedMultiplier -= 0.05f;
        playerSpeedMultiplier = Mathf.Clamp(playerSpeedMultiplier, 0.3f, 1f);
        
        if (directionForward)
            OnPlayerMove?.Invoke();
        else
            OnPlayerMoveBackwards?.Invoke();
        
        gameObject.transform.Rotate(0, (directionForward ? 1 : -1) * (360f / 60f), 0);
        
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