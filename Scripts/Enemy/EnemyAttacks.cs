using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [Header("States")]
    public StateManager stateManager;
    public AttackState attackState;

    [Header("Enemy Attacks")]
    public EnemyActionTypes enemyActions;

    bool isAttacking;
    bool isTakingDamage;

    Animator anim;
    EnemyManager enemyManager;

    void Start ()
    {
        anim = GetComponentInChildren<Animator>();

        enemyManager = GetComponent<EnemyManager>();
    }

    void Update ()
    {
        isAttacking = anim.GetBool("isAttacking");

        if (!enemyManager.enemyType.isBoss)
        {
            isTakingDamage = anim.GetBool("isTakingDamage");
        }

        if (stateManager.currentState == attackState)
        {
            if (isAttacking)
                return;

            if (isTakingDamage)
                return;

            if (attackState.canParry)
                return;

            int numOfEnemyAttacks = enemyActions.enemyAttacks.Count;

            int randomAttackPicker = Random.Range(0, numOfEnemyAttacks);

            anim.SetBool("isAttacking", true);
            anim.CrossFade(enemyActions.enemyAttacks[randomAttackPicker], 0.1f);
        }
    }
}
