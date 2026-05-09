using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public EnemyManager enemy;
    public BossHealthManager bossHealthManager;
    public EnemyHealth enemyHealth;
    public DeadState deadState;

    public override State RunCurrentState()
    {
        if (enemyHealth.isDead || enemyHealth.currentHealth <= 0f)
            return deadState;

        #region Handle Switching States

        //switches between idle then chasing
        if (Physics.CheckSphere(enemy.transform.position, enemy.enemyType.detectionRadius, enemy.targetLayer))
        {
            if (bossHealthManager != null)
            {
                bossHealthManager.ActivateBossHealthBar();
            }

            return chaseState;
        }
        else
        {
            return this;
        }

        #endregion
    }
}
