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
    HitStop hitStop;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        enemy = GetComponentInParent<EnemyManager>();

        parry = FindObjectOfType<Parry>();
        dodge = FindObjectOfType<Dodge>();
        soundManager = FindObjectOfType<SoundManager>();
        hitStop = FindObjectOfType<HitStop>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!parry.parrying)
            {
                if (dodge.isInvincible)
                {
                    if (Vector3.Distance(dodge.transform.position, enemy.transform.position) > 0.5f)
                    {
                        Invoke("HandleEffects", 0.045f);
                    }

                    return;
                }

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

    void HandleEffects()
    {
        hitStop.HitStopEffect(0.3f, 0.09f);
    }
}
