using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] public float health;
    public float sightRange, attackTimer, attackRange, damageValue;
    protected float attackTimer2 = 0;
    protected bool attackCooldown = true;
    public float angle, speedToLook;
    protected Rigidbody rb;

    public Transform player;
    public LayerMask isPlayer, whatIsGround;

    [HideInInspector] public Animator animator;

    public bool PlayerInSightRange()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToPlayer) <= angle / 2)
        {
            if (Physics.Raycast(transform.position, directionToPlayer, sightRange, isPlayer))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerInRange()
    {
        return Physics.CheckSphere(transform.position, attackRange, isPlayer);
    }

    protected virtual void EnemyDeath()
    {
        animator.SetTrigger("Death");
        Destroy(gameObject, 4);
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0) EnemyDeath();
        }
    }
}
