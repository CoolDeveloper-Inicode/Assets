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

    public void DetectEnemy()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation);

        foreach (var other in colliders)
        {
            if (other.CompareTag("Enemy"))
            {
                AttackState attackState = other.GetComponentInChildren<AttackState>();

                if (attackState.canParry)
                {
                    parry.HasBeenParried();
                    other.GetComponentInChildren<Animator>().Play("ParryHit");
                }
                else
                {
                    playerCombatSystem.canTp = false;

                    if (other.GetComponent<EnemyHealth>().isDead)
                        return;

                    if (other.GetComponent<EnemyHealth>().isInvincible)
                        return;

                    other.GetComponent<EnemyHealth>().TakeDamage(playerCombatSystem.currentWeapon.damage);

                    //handle the effects
                    StartCoroutine(cameraShake.Shake(0.1f, 0.14f));
                    hitStop.HitStopEffect(0.04f, 0.01f);

                    other.GetComponent<Rigidbody>().AddForce(effectsManager.playerObj.forward * 90f, ForceMode.Impulse);
                    rb.AddForce(effectsManager.playerObj.forward * 25f, ForceMode.Impulse);

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
