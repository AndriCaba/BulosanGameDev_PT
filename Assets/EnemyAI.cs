using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public int damage = 10;
    public float attackCooldown = 2f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float retreatDistance = 2f;
    public float separationDistance = 1f; // Minimum distance between enemies

    [Header("Damage Effects")]
    public float knockbackForce = 5f; // Force applied when taking damage
    public float flashDuration = 0.1f; // Duration of red flash effect

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("States")]
    private bool isFacingRight = true;
    private bool isInCooldown;
    private bool isFlashing = false; // Track if the enemy is already flashing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            Debug.LogWarning("Player reference not set. Please assign the player in the inspector.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isInCooldown)
        {
            RetreatFromPlayer(distanceToPlayer);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            FlipTowardsPlayer();

            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            Idle();
        }

        // Avoid collision with other enemies
        AvoidCollisionsWithEnemies();
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void AttackPlayer()
    {
        animator.SetBool("isWalking", false);

        if (!isInCooldown)
        {
            animator.SetTrigger("isAttacking");
            StartCoroutine(AttackCooldown());

            // Apply damage to the player if they have a health component
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position);
            }
            else
            {
                Debug.LogWarning("Player does not have a PlayerHealth component.");
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isInCooldown = false;
    }

    void RetreatFromPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer < retreatDistance)
        {
            animator.SetBool("isWalking", true);

            Vector2 direction = (transform.position - player.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            Idle();
        }
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        float directionToPlayer = player.position.x - transform.position.x;

        if ((directionToPlayer > 0 && !isFacingRight) || (directionToPlayer < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX; // Flip the sprite
        }
    }

    void Idle()
    {
        animator.SetBool("isWalking", false);
        rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement
    }

    // Avoid collisions with other enemies
    void AvoidCollisionsWithEnemies()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance, LayerMask.GetMask("enemyLayer"));

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy != this.GetComponent<Collider2D>()) // Avoid self-check
            {
                Vector2 directionAwayFromEnemy = (transform.position - enemy.transform.position).normalized;
                rb.velocity += directionAwayFromEnemy * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        // Flash red when taking damage
        if (!isFlashing)
        {
            StartCoroutine(FlashRed());
        }

        // Knockback effect
        Knockback(attackerPosition);

        // Optional: Add logic for reducing health or handling death here
        Debug.Log($"Enemy took {damage} damage!");
    }

    void Knockback(Vector2 attackerPosition)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    IEnumerator FlashRed()
    {
        isFlashing = true;

        // Change color to red
        spriteRenderer.color = Color.red;

        // Wait for flash duration
        yield return new WaitForSeconds(flashDuration);

        // Reset color to default
        spriteRenderer.color = Color.white;
        isFlashing = false;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Visualize detection and attack ranges
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
