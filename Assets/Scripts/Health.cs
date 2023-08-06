using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    
    private int currentHealth;
    
    public Action<int> OnHealthChanged;

    private void Awake()
    {
        FullHeal();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
            Die();
    }
    
    public void FullHeal()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
