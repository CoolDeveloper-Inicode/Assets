using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Weapon")]
    public Weapon currentWeapon;

    [Header("Light Attack Variables")]
    float attackTimer;
    int lightAttackComboCounter;

    [Header("Heavy Attack Variables")]
    int heavyAttackComboCounter;

    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool canTp;

    [Header ("Scripts")]
    PlayerAnimatorController playerAnimatorController;
    PlayerMovementTutorial playerMovementTutorial;
    Animator anim;
    SoundManager soundManager;
    Parry parry;
    PlayerHealing playerHealing;
    SkillArtUI skillArtUI;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerAnimatorController = GetComponentInChildren<PlayerAnimatorController>();

        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        parry = GetComponent<Parry>();
        playerHealing = GetComponent<PlayerHealing>();
        skillArtUI = GetComponent<SkillArtUI>();

        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (canTp)
        {
            Invoke(nameof(ResetBools), 0.1f);
        }

        if (playerHealing.isHealing)
            return;

        if (parry.hasBeenParried)
            return;

        if (!playerMovementTutorial.grounded)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleLightAttacks();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            HandleHeavyAttack();
        }

        if (skillArtUI.currentSkillAmount >= skillArtUI.maxSkillAmount)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                HandleSkillArt();
            }
        }

        #region Handle Boolean

        isAttacking = anim.GetBool("isAttacking");

        if (playerAnimatorController.canRoll)
        {
            anim.SetBool("isAttacking", false);
        }
        
        #endregion

        #region Reseting Attacks

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            lightAttackComboCounter = 0;
            heavyAttackComboCounter = 0;
        }

        #endregion
    }

    #region Handle Attacks

    void HandleLightAttacks()
    {
        if (!playerAnimatorController.canDoCombo)
            return;

        playerAnimatorController.canDoCombo = false;
        canTp = true;
        anim.SetBool("isAttacking", true);
        playerAnimatorController.PlayTargetAnimation(currentWeapon.lightAttackAnimations[lightAttackComboCounter], true);
        lightAttackComboCounter++;
        currentWeapon.damage = currentWeapon.lightAttackDamage;
        attackTimer = 0.8f;

        //handle effects
        Invoke("PlaySoundEffect", 0.1f);

        if (lightAttackComboCounter >= currentWeapon.lightAttackAnimations.Count)
        {
            lightAttackComboCounter = 0;
        }
    }

    void HandleHeavyAttack()
    {
        if (!playerAnimatorController.canDoCombo)
            return;           

        playerAnimatorController.canDoCombo = false;
        canTp = true;
        anim.SetBool("isAttacking", true);
        playerAnimatorController.PlayTargetAnimation(currentWeapon.heavyAttackAnimations[heavyAttackComboCounter], true);
        heavyAttackComboCounter++;
        currentWeapon.damage = currentWeapon.heavyAttackDamage;
        attackTimer = 0.8f;

        if (heavyAttackComboCounter >= currentWeapon.heavyAttackAnimations.Count)
        {
            heavyAttackComboCounter = 0;
        }
    }

    void HandleSkillArt()
    {
        playerAnimatorController.canDoCombo = false;
        anim.SetBool("isAttacking", true);
        playerAnimatorController.PlayTargetAnimation(currentWeapon.skillAttackAnimation, true);
        currentWeapon.damage = currentWeapon.skillAttackDamage;

        //handle UI
        skillArtUI.currentSkillAmount = 0f;
        skillArtUI.playerUI.SetCurrentHealth(skillArtUI.currentSkillAmount);
    }

    #endregion

    #region Handle Misc

    void PlaySoundEffect()
    {
        soundManager.PlayTargetSound(soundManager.audioSource, soundManager.swordSwingSFX);
    }

    void ResetBools()
    {
        canTp = false;
    }

    #endregion
}
