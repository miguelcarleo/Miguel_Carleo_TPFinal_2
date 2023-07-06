using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieIA : BaseEnemy
{
    [HideInInspector] public float velocity;

    [SerializeField] private float speed, runningSpeed;
    private bool dead = false;

    [HideInInspector] public NavMeshAgent agent;
    public float range;

    public Transform centrePoint;

    AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip runClip;

    private float runIdleTime = 7;
    private float runIdleTime2;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.angularSpeed = speedToLook;

        runIdleTime2 = runIdleTime;
    }
    private void Update()
    {
        if (!dead)
        {
            if (attackCooldown)
            {
                if (PlayerInSightRange())
                {
                    if (runIdleTime2 <= 0)
                    {
                        PlaySound(runClip);
                        runIdleTime2 = runIdleTime;
                    }
                    ChasePlayer();
                    animator.SetBool("Walking", false);
                    animator.SetBool("Running", true);
                    if (PlayerInRange())
                    {
                        PlaySound(hitClip);
                        animator.SetTrigger("Attacking");
                        attackTimer2 = attackTimer;
                        attackCooldown = false;
                        player.GetComponent<PlayerHealth>().Damage(damageValue);
                    }
                }
                else
                {
                    animator.SetBool("Running", false);
                    animator.SetBool("Walking", true);
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        Vector3 point;
                        if (RandomPoint(centrePoint.position, range, out point))
                        {
                            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                            agent.SetDestination(point);
                        }
                    }
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        if (!attackCooldown)
        {
            attackTimer2 -= Time.deltaTime;
            if (attackTimer2 <= 0) attackCooldown = true;
        }
        runIdleTime2 -= Time.deltaTime;
    }

    private void ChasePlayer()
    {
        var distance = player.position - transform.position;
        Quaternion Rotation = Quaternion.LookRotation((player.position - transform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, Time.deltaTime * speedToLook);
        if (attackCooldown)
        {
            transform.position += distance.normalized * Time.deltaTime * runningSpeed;
        }
    }

    protected override void EnemyDeath()
    {
        dead = true;
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Death");
        Destroy(gameObject, 4);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void PlaySound(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
}
