using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    [Header("Scripts")]

    public PlayerController movement;

    [Header("Health Settings")]
    public int maxHealth = 100; // The player's maximum health
    public int currentHealth; // The player's current health

    [Header("UI")]
    public Healthbar healthBar; // Optional reference to a UI health bar

    [Header("Damage Effects")]
    public float knockbackForce = 5f; // Force applied when damaged
    public float flashDuration = 0.1f; // Duration of the red flash effect

    private SpriteRenderer spriteRenderer; // Reference to the player's sprite renderer
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private bool isFlashing = false; // Track if the player is already flashing

    void Start()
    {
        currentHealth = maxHealth; // Set health to max at the start
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerController>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // Initialize the health bar
        }
    }
    void Update(){


if(Input.GetKeyDown(KeyCode.K)){
    Die();

}

    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update health bar
        }

        // Trigger knockback
        Knockback(attackerPosition);

        // Trigger flash effect
        if (!isFlashing)
        {
            StartCoroutine(FlashRed());
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

    void Knockback(Vector2 attackerPosition)
    {
        // Calculate knockback direction
        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    IEnumerator FlashRed()
    {
        movement.enabled = false;
        isFlashing = true;

        // Change color to red
        spriteRenderer.color = Color.red;

        // Wait for flash duration
        yield return new WaitForSeconds(flashDuration);
        movement.enabled = true;
        // Reset color to default
        spriteRenderer.color = Color.white;
        isFlashing = false;
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Player has died!");
        // Add player death logic here, such as restarting the level or triggering animations
        Destroy(gameObject); // Destroy the player object (optional)
    }
}
