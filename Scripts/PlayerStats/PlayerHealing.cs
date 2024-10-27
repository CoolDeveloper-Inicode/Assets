using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealing : MonoBehaviour
{
    [Header("Healing Properties")]
    public float healAmount;
    public GameObject potionModel;
    public TextMeshProUGUI potionAmount;

    [HideInInspector]
    public bool isHealing;
    [HideInInspector]
    public int amountOfPotions;

    PlayerAnimatorController playerAnimatorController;
    Animator anim;
    PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        playerAnimatorController = GetComponentInChildren<PlayerAnimatorController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (amountOfPotions <= 0)
                return;

            if (isHealing)
               return;

            if (playerStats.currentHealth >= playerStats.maxHealth)
                return;

            anim.SetBool("isHealing", true);
            playerAnimatorController.PlayTargetAnimation("Heal", false);
            potionModel.SetActive(true);
        }

        isHealing = anim.GetBool("isHealing");

        potionAmount.text = amountOfPotions.ToString();
    }

    public void Healing()
    {
        if (playerStats.currentHealth + healAmount < playerStats.maxHealth)
        {
            playerStats.HealPlayer(healAmount);
        }
        else
        {
            playerStats.HealPlayer(playerStats.maxHealth - playerStats.currentHealth);
        }

        amountOfPotions--;
        Invoke(nameof(DisablePotion), 0.7f);
    }

    void DisablePotion()
    {
        if (isHealing)
            return;
        
        potionModel.SetActive(false);
    }
}
