using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterIA : BaseEnemy
{
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform castPoint;
    [SerializeField] float fireBallVelocity;
    private bool dead = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerInSightRange() && !dead)
        {
            LookAtPlayer();
            if (PlayerInRange() && attackCooldown)
            {
                animator.SetTrigger("Attack");
                FireBall();
                attackTimer2 = attackTimer;
                attackCooldown = false;
            }
        }

        if (!attackCooldown)
        {
            attackTimer2 -= Time.deltaTime;
            if (attackTimer2 <= 0) attackCooldown = true;
        }
    }

    private void LookAtPlayer()
    {
        Quaternion Rotation = Quaternion.LookRotation((player.position - transform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime * speedToLook);
    }

    private void FireBall()
    {
        GameObject currentFireBall = Instantiate(fireBall, castPoint.position, castPoint.rotation);
        currentFireBall.GetComponent<FireBall>().SetDamage(damageValue);
        Rigidbody rb = currentFireBall.GetComponent<Rigidbody>();
        rb.AddForce(castPoint.forward * fireBallVelocity, ForceMode.Impulse);
    }

    protected override void EnemyDeath()
    {
        dead = true;
        animator.SetTrigger("Death");
        Destroy(gameObject, 4);
    }
}
