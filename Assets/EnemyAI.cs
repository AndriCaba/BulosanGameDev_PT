using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float speed = 2f;
    public int damage = 10;
    public float attackCooldown = 2f;

    private Animator animator;
    private float lastAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            // Move towards the player
         //   animator.SetBool("isWalking", true);
          //  animator.SetBool("isAttacking", false);

            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            // Attack the player
        //    animator.SetBool("isWalking", false);
          //  animator.SetBool("isAttacking", true);

            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Enemy attacks the player!");
        // Assume the player has a script called PlayerHealth
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void Idle()
    {
       // animator.SetBool("isWalking", false);
       // animator.SetBool("isAttacking", false);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize detection and attack ranges
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
