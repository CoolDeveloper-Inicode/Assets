using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    Animator anim;
    EnemyManager enemy;
    Parry parry;
    SoundManager soundManager;
    Dodge dodge;
    EnemyHealth enemyHealth;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        enemy = GetComponentInParent<EnemyManager>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        parry = FindObjectOfType<Parry>();
        dodge = FindObjectOfType<Dodge>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!parry.parrying)
            {
                if (dodge.isInvincible)
                    return;

                other.GetComponent<PlayerStats>().TakeDamage(enemy.enemyType.damage);
                soundManager.PlayTargetSound(soundManager.lowVolumeAudioSource, soundManager.hitSwordSFX);
            }
            else
            {
                parry.Parried();
                parry.RotatePlayerToTarget(enemy.transform);
                anim.Play("Deflect");
            }
        }
    }
}
