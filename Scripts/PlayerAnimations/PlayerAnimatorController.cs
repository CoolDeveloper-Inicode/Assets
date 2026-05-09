using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [HideInInspector]
    public bool canRoll;
    [HideInInspector]
    public bool canDoCombo;
    [HideInInspector]
    public bool canLaunchUp;

    Animator anim;
    Rigidbody rb;
    SoundManager soundManager;
    EffectsManager effectsManager;
    PlayerDamage playerDamage;
    PlayerHealing playerHealing;
    PlayerCombatSystem playerCombatSystem;
    PlayerMovementTutorial playerMovementTutorial;

    void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponentInParent<Rigidbody>();
        effectsManager = GetComponentInParent<EffectsManager>();
        playerHealing = GetComponentInParent<PlayerHealing>();
        playerCombatSystem = GetComponentInParent<PlayerCombatSystem>();
        playerMovementTutorial = GetComponentInParent<PlayerMovementTutorial>();

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

    public void JumpUp()
    {
        playerMovementTutorial.TakeOf();
    }

    public void ResetSkillBool()
    {
        anim.SetBool("isSkillAttack", false);
    }

    public void LaunchEnemy()
    {
        canLaunchUp = true;
    }

    public void LaunchPlayer()
    {
        rb.AddForce(playerMovementTutorial.transform.up * 40f, ForceMode.Impulse);
    }

    public void DisableLaunchUp()
    {
        canLaunchUp = false;
    }

    public void SetLaunchBool()
    {
        anim.SetBool("isLaunching", false);
    }

    public void GroundPlayer()
    {
        rb.AddForce(-playerMovementTutorial.transform.up * 100f, ForceMode.Impulse);
    }

    public void SetGroundSlamBool()
    {
        anim.SetBool("isGroundSlam", false);
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

        if (anim.GetBool("isLaunching"))
        {
            velocity.y = rb.velocity.y;
        }

        rb.velocity = velocity;
    }

    #endregion
}
