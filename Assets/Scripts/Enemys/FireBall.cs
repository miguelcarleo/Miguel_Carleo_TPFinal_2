using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    private float damageValue;
    AudioSource audioSource;

    [SerializeField] AudioClip audioclip;


    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(damageValue);
        }
    }

    public void SetDamage(float damage)
    {
        damageValue = damage;
    }
}
