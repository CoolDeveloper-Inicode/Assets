using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [HideInInspector]
    public bool canRoll;
    [HideInInspector]
    public bool canDoCombo;

    Animator anim;
    Rigidbody rb;
    SoundManager soundManager;
    EffectsManager effectsManager;
    PlayerDamage playerDamage;
    PlayerHealing playerHealing;
    PlayerCombatSystem playerCombatSystem;

    void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponentInParent<Rigidbody>();
        effectsManager = GetComponentInParent<EffectsManager>();
        playerHealing = GetComponentInParent<PlayerHealing>();
        playerCombatSystem = GetComponentInParent<PlayerCombatSystem>();

        playerDamage = GetComponentInChildren<PlayerDamage>();

        soundManager = FindObjectOfType<SoundManager>();

        canDoCombo = true;
    }

    void Update()
    {
        canRoll = anim.GetBool("canRoll");
    }

    #region Animation Events

    public void DetectForEnemy()
    {
        playerDamage.DetectEnemy();
    }

    public void PlayRollSFX()
    {
        soundManager.PlayTargetSound(soundManager.lowVolumeAudioSource, soundManager.rollSFX);
    }

    public void PlaySwordSwingSFX()
    {
        soundManager.PlayTargetSound(soundManager.audioSource, soundManager.swordSwingSFX);
    }

    public void EnableRollCancel()
    {
        anim.SetBool("canRoll", true);
    }

    public void InstantiateWeaponTrail()
    {
        Instantiate(effectsManager.weaponTrail, effectsManager.weaponTrailSpawn.position, effectsManager.weaponTrailSpawn.rotation);
    }

    public void EnableCombo()
    {
        canDoCombo = true;
    }

    public void DisableCombo()
    {
        canDoCombo = false;
    }

    public void TpPlayer()
    {
        playerCombatSystem.canTp = true;
    }

    public void HealPotion()
    {
        playerHealing.Healing();
    }

    #endregion

    #region Functions

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("IsInteracting", isInteracting);
        anim.CrossFadeInFixedTime(targetAnimation, 0.1f);
    }

    private void OnAnimatorMove()
    {
        if(Mathf.Approximately(Time.deltaTime, 0f)) 
            return;

        if (!anim.GetBool("IsInteracting"))
            return;

        float delta = Time.deltaTime;
        rb.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        Vector3 velocity = deltaPosition / delta;
        rb.velocity = velocity;
    }

    #endregion
}
