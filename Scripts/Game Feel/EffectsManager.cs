using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [Header("Parry Effects")]
    public GameObject parryEffect;
    public Transform parryEffectSpawn;

    [Header("Weapon Effects")]
    public GameObject weaponTrail;
    public Transform weaponTrailSpawn;

    [Header("Player Object")]
    public Transform playerObj;

    public void PlayTargetEffect(ParticleSystem targetEffect)
    {
        targetEffect.Play();
    }

    public void SpawnTargetEffect(GameObject targetEffect, Transform targetSpawn)
    {
        Instantiate(targetEffect, targetSpawn.position, Quaternion.identity);
    }
}
