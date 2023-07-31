using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeToLive = 5;
    private float damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Rotate(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        /// Destroy the bullet after a certain amount of time
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
            Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        /// If the bullet hits something, deal damage to it
        DealDamage(collision.gameObject);
    }
    
    void DealDamage(GameObject otherGameObject)
    {
        // gameObject.GetComponent<Health>().TakeDamage(damage);
        /// Destroy the bullet when it hits something
        Destroy(gameObject);
    }
}
