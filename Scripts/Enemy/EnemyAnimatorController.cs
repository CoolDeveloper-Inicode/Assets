using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    Collider coll;
    SoundManager soundManager;
    EnemyManager enemyManager;
    EnemyHealth enemyHealth;
    PlayerAnimatorController playerAnimController;

    void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponentInParent<Rigidbody>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyHealth = GetComponentInParent<EnemyHealth>();


        coll = GetComponentInChildren<Collider>();

        soundManager = FindObjectOfType<SoundManager>();
        playerAnimController = FindObjectOfType<PlayerAnimatorController>();

        anim.SetBool("isAirHit", false);
    }

    #region Animation Events

    public void EnableDamageCollider()
    {
        coll.enabled = true;
    }

    public void DisableDamageCollider()
    {
        coll.enabled = false;
    }

    public void InstantiateWeaponTrail()
    {
        Instantiate(enemyManager.weaponTrailEffect, enemyManager.weaponTrailEffectSpawn.position, enemyManager.weaponTrailEffectSpawn.rotation);
    }

    public void PlaySwordEffects()
    {
        soundManager.PlayTargetSound(soundManager.audioSource, soundManager.swordSwingSFX);
    }

    public void PerfectDodgeDetector()
    {
        StartCoroutine(PerfectDodge());
    }

    public void TpEnemy()
    {
        if (Vector3.Distance(enemyManager.transform.position, enemyManager.targetTransform.position) >= 2.3f)
        {
            enemyManager.rb.AddForce(enemyManager.transform.forward * 70f, ForceMode.Impulse);
        }
    }

    public void DisableLaunch()
    {
        playerAnimController.canLaunchUp = false;
    }

    public void IsInvincible()
    {
        enemyHealth.isInvincible = true;
    }

    public void IsntInvincible()
    {
        enemyHealth.isInvincible = false;
    }

    #endregion

    #region Functions

    private void OnAnimatorMove()
    {
        if (!anim.applyRootMotion)
            return;

        if(Mathf.Approximately(Time.deltaTime, 0f)) 
            return;

        float delta = Time.deltaTime;
        rb.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        Vector3 velocity = deltaPosition / delta;

        if (!anim.GetBool("isAirHit"))
        {
            velocity.y = rb.velocity.y;
        }

        rb.velocity = velocity;
    }

    IEnumerator PerfectDodge()
    {
        enemyManager.perfectDodgeWindow = true;

        yield return new WaitForSeconds(0.05f);

        enemyManager.perfectDodgeWindow = false;
    }

    #endregion
}
