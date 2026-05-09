using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public float parryWindowTime;
    public float parryRecoveryTime;

    [HideInInspector]
    public bool parrying;
    [HideInInspector]
    public bool hasBeenParried;
    [HideInInspector]
    public bool hasPerformedParry;

    float parryTimer;

    PlayerAnimatorController playerAnimatorController;
    PlayerMovementTutorial playerMovementTutorial;
    Animator anim;
    EffectsManager effectsManager;
    SoundManager soundManager;
    HitStop hitStop;
    PlayerHealing playerHealing;
    Dodge dodge;

    void Start()
    {
        playerAnimatorController = GetComponentInChildren<PlayerAnimatorController>();
        anim = GetComponentInChildren<Animator>();

        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        effectsManager = GetComponent<EffectsManager>();
        hitStop = GetComponent<HitStop>();
        playerHealing = GetComponent<PlayerHealing>();
        dodge = GetComponent<Dodge>();

        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (playerHealing.isHealing)
            return;

        if (hasPerformedParry)
            return;
        
        if (!playerMovementTutorial.grounded)
            return;

        if (dodge.isDodging)
            return;

        if (hasBeenParried)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer <= 0)
            {
                hasBeenParried = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            hasPerformedParry = true;
            parrying = true;
            hasBeenParried = false;
            playerAnimatorController.PlayTargetAnimation("Parry", true);
            StartCoroutine(ResetParry());
            StartCoroutine(CanParryAgain());
        }
    }

    #region Parrying Coroutines

    IEnumerator ResetParry()
    {
        yield return new WaitForSeconds(parryWindowTime);

        parrying = false;
    }

    IEnumerator CanParryAgain()
    {
        yield return new WaitForSeconds(0.15f);

        hasPerformedParry = false;
    }

    #endregion

    #region Parrying Functions

    public void Parried()
    {
        playerAnimatorController.PlayTargetAnimation("ParryHit", true);

        //handle the effects
        Invoke("SpawnEffectWithSlowMo", 0.11f);
        //handle sounds
        hasPerformedParry = true;
        StartCoroutine(CanParryAgain());
        parrying = false;
        soundManager.PlayTargetSound(soundManager.audioSource, soundManager.parrySFX);
    }

    public void HasBeenParried()
    {
        //handle animation
        anim.SetBool("isAttacking", false);
        //handle effects
        Invoke("SpawnEffect", 0.11f);
        //handle sounds
        soundManager.PlayTargetSound(soundManager.audioSource, soundManager.parrySFX);

        hasBeenParried = true;
        parryTimer = parryRecoveryTime;
    }

    public void RotatePlayerToTarget(Transform targetTransform)
    {
        Vector3 targetPosition = targetTransform.position - dodge.playerObj.position;
        targetPosition.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

        dodge.playerObj.rotation = Quaternion.Slerp(dodge.playerObj.rotation, targetRotation, 1f);
    }

    #endregion

    #region Handle Effects

    void SpawnEffectWithSlowMo()
    {
        hitStop.HitStopEffect(0.2f, 0.15f);
        effectsManager.SpawnTargetEffect(effectsManager.parryEffect, effectsManager.parryEffectSpawn);
    }

    void SpawnEffect()
    {
        effectsManager.SpawnTargetEffect(effectsManager.parryEffect, effectsManager.parryEffectSpawn);
    }

    #endregion
}
