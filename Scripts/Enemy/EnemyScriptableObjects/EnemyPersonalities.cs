using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Create New Enemy")]
public class EnemyPersonalities : ScriptableObject
{
    public string enemyName;

    public float detectionRadius;

    public float strafingDistance;

    public float attackingDistance;

    public float lungingAttackDistance;

    public float nearAttackDistance;

    public float damage;

    public float dodgingLikelyHood;

    public float parryLikelyHood;

    public float blockingLimit;

    public float rotationSpeed;

    public bool isAggresive;

    public bool isBoss;
}
