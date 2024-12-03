using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    public int health = 20;
    public float knockbackForce = 5f;
    public float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public EnemyAI Movementscript;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Movementscript = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>(); // Ensure your enemy GameObject has an Animator component
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        // Reduce health
        health -= damage;
        Debug.Log("Enemy took damage! Remaining health: " + health);

        // Apply knockback
        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // Flash white
        StartCoroutine(FlashWhite());

        // Check for death
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashWhite()
    {
        Movementscript.enabled = false;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white; // Reset to original color
        Movementscript.enabled = true;
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Trigger the death animation
        animator.SetTrigger("Die");

        // Disable enemy functionality
        Movementscript.enabled = false;
        rb.velocity = Vector2.zero; // Stop movement

        // Destroy the enemy after the animation finishes
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
