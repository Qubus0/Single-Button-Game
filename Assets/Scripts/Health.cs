using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float invulnerabilityTime = 0.1f;

    private int currentHealth;
    private bool invlunerable = false;
    
    public Action<int> OnHealthChanged;

    private void Awake()
    {
        FullHeal();
    }

    public void TakeDamage(int damage)
    {
        if (invlunerable) return;
        invlunerable = true;
        Invoke(nameof(MakeVulnerable), invulnerabilityTime);
        
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
            Die();
    }
    
    private void MakeVulnerable()
    {
        invlunerable = false;
    }
    
    public int GetHealth()
    {
        return currentHealth;
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
