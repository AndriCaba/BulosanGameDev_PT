using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // The player's maximum health
    public int currentHealth; // The player's current health
    public Healthbar healthBar; // Optional reference to a UI health bar

    void Start()
    {
        currentHealth = maxHealth; // Set health to max at the start
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Initialize the health bar
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update health bar
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Cap health at maxHealth
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update health bar
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Add player death logic here, such as restarting the level
        Destroy(gameObject); // This will destroy the player object (optional)
    }
}
