using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [HideInInspector] public float currentHealth;
    public GameObject death;
    public Image currentHealthBar;
    public GameObject healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void Damage(float damage)
    {
        currentHealth -= damage;
        currentHealthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Update()
    {
        if (GameManager.isActive)
        {
            healthBar.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            GameManager.isActive = false;
            GameManager.UnLockCursor();
            Time.timeScale = 0;
            death.gameObject.SetActive(true);
        }
    }
}
