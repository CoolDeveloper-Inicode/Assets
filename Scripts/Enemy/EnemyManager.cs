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

    [HideInInspector]
    public bool perfectDodgeWindow;

    [HideInInspector]
    public Rigidbody rb;

    ChaseState chaseState;
    CameraMovement cameraMovement;
    HitStop hitStop;
    Dodge dodge;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        chaseState = GetComponentInChildren<ChaseState>();

        cameraMovement = FindObjectOfType<CameraMovement>();
        hitStop = FindObjectOfType<HitStop>();
        dodge = FindObjectOfType<Dodge>();
    }

    void Update()
    {
        if (transform != cameraMovement.lockOnTargetTransform)
        {
            chaseState.chosenOne = false;
        }

        if (perfectDodgeWindow)
        {
            if (dodge.isDodging)
            {
                if (Vector3.Distance(dodge.transform.position, transform.position) <= 2f)
                {
                    dodge.isInvincible = true;
                    hitStop.HitStopEffect(0.2f, 0.089f);
                }
            }
        }
    }
}
