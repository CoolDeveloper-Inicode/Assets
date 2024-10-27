using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Properties")]
    public EnemyPersonalities enemyType;

    [Header("Enemy Target")]
    public LayerMask targetLayer;
    public Transform targetTransform;

    [Header("Effects")]
    public GameObject weaponTrailEffect;
    public Transform weaponTrailEffectSpawn;

    ChaseState chaseState;
    CameraMovement cameraMovement;

    void Start()
    {
        chaseState = GetComponentInChildren<ChaseState>();

        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    void Update()
    {
        if (transform != cameraMovement.lockOnTargetTransform)
        {
            chaseState.chosenOne = false;
        }
    }
}
