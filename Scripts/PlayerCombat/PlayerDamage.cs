using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Scripts")]
    PlayerCombatSystem playerCombatSystem;
    PlayerAnimatorController playerAnimatorController;
    Parry parry;
    SoundManager soundManager;
    EffectsManager effectsManager;
    Rigidbody rb;
    HitStop hitStop;
    CameraShake cameraShake;

    [HideInInspector]
    public float blockingTime;

    void Start()
    {
        playerCombatSystem = GetComponentInParent<PlayerCombatSystem>();
        playerAnimatorController = GetComponentInParent<PlayerAnimatorController>();
        parry = GetComponentInParent<Parry>();
        effectsManager = GetComponentInParent<EffectsManager>();
        rb = GetComponentInParent<Rigidbody>();
        hitStop = GetComponentInParent<HitStop>();

        soundManager = FindObjectOfType<SoundManager>();
        cameraShake = FindObjectOfType<CameraShake>();
    }

    void Update()
    {
        if (blockingTime > 0)
        {
            blockingTime -= Time.deltaTime;
        }
    }

    public void DetectEnemy()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation);

        foreach (var other in colliders)
        {
            if (other.CompareTag("Enemy"))
            {
                AttackState attackState = other.GetComponentInChildren<AttackState>();
                EnemyManager enemyManager = other.GetComponent<EnemyManager>();
                EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
                Rigidbody enemyRB = other.GetComponent<Rigidbody>();
                Animator enemyAnim = other.GetComponentInChildren<Animator>();
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

                if (attackState.canParry && enemyMovement.grounded)
                {
                    //ENEMY BLOCKING
                    if (enemyManager.blockAmount >= enemyManager.enemyType.blockingLimit)
                    {
                        enemyManager.isBlocking = false;
                        enemyManager.blockAmount = 0;

                        attackState.canParry = false;
                        attackState.rollForParryChance = false;
                    }
                    else
                    {
                        enemyManager.isBlocking = true;
                        blockingTime = 0.6f;
                        enemyManager.blockAmount++;

                        //function
                        parry.HasBeenParried();

                        //animation
                        enemyAnim.Play("Block");

                        //shake
                        StartCoroutine(cameraShake.Shake(0.1f, 0.15f));

                        //knockback
                        enemyRB.AddForce(effectsManager.playerObj.forward * 60f, ForceMode.Impulse);
                        rb.AddForce(effectsManager.playerObj.forward * 30f, ForceMode.Impulse);
                    }
                }
                else
                {
                    //ENEMY DAMAGING OR LAUNCHING
                    playerCombatSystem.canTp = false;

                    if (enemyHealth.isDead)
                        return;

                    if (enemyHealth.isInvincible)
                        return;

                    enemyHealth.TakeDamage(playerCombatSystem.currentWeapon.damage);

                    //handle the effects
                    StartCoroutine(cameraShake.Shake(0.1f, 0.15f));
                    hitStop.HitStopEffect(0.04f, 0.015f);

                    if (!playerAnimatorController.canLaunchUp)
                    {
                        enemyRB.AddForce(effectsManager.playerObj.forward * 60f, ForceMode.Impulse);
                    }

                    rb.AddForce(effectsManager.playerObj.forward * 30f, ForceMode.Impulse);

                    //handle the sound
                    soundManager.PlayTargetSound(soundManager.lowVolumeAudioSource, soundManager.hitSwordSFX);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
