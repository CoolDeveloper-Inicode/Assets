using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth;

    public PlayerUI playerUI;

    [HideInInspector]
    public float currentHealth;

    bool isDead;

    PlayerAnimatorController playerAnimatorController;
    Animator anim;

    void Start()
    {
        playerAnimatorController = GetComponentInChildren<PlayerAnimatorController>();
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
        playerUI.SetMaxHealth(maxHealth);
        playerUI.SetCurrentHealth(currentHealth);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            anim.Play("Dead");
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        playerUI.SetCurrentHealth(currentHealth);

        playerAnimatorController.PlayTargetAnimation("TakeDamage", true);
    }

    public void HealPlayer(float regenerationAmount)
    {
        currentHealth += regenerationAmount;
        playerUI.SetCurrentHealth(currentHealth);
    }
}
