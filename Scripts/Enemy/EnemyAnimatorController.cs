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

    void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponentInParent<Rigidbody>();
        enemyManager = GetComponentInParent<EnemyManager>();

        coll = GetComponentInChildren<Collider>();

        soundManager = FindObjectOfType<SoundManager>();
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

    #endregion

    #region Functions

    private void OnAnimatorMove()
    {
        if(Mathf.Approximately(Time.deltaTime, 0f)) 
            return;

        float delta = Time.deltaTime;
        rb.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        Vector3 velocity = deltaPosition / delta;
        rb.velocity = velocity;
    }

    #endregion
}
