using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public Animator anim;
    public EnemyManager enemy;
    public CombatStanceState combatStanceState;
    public AttackState attackState;
    public EnemyHealth enemyHealth;
    public DeadState deadState;

    [HideInInspector]
    public bool chosenOne;

    bool rollForStrafeChance;
    float horizontalMovement;

    public override State RunCurrentState()
    {
        if (enemyHealth.isDead)
            return deadState;

        //resseting the strafing chance
        combatStanceState.rollForStrafeChance = false;

        float distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.targetTransform.position);

        //rotate to look at the target
        RotateTowardsTarget();

        if (chosenOne)
        {
            //chases the target
            anim.SetFloat("Vertical", 2f, 0.2f, Time.deltaTime);
        }
        else
        {
            //determines whether the enemy should strafe left or right
            if (!rollForStrafeChance)
            {
                horizontalMovement = Random.Range(1, 3);
                rollForStrafeChance = true;
            }

            //plays strafing animation
            if (horizontalMovement == 2)
            {
                anim.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                anim.SetFloat("Horizontal", 0.5f, 0.2f, Time.deltaTime);
            }
            else if (horizontalMovement == 1)
            {
                anim.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                anim.SetFloat("Horizontal", -0.5f, 0.2f, Time.deltaTime);
            }
        }

        #region Handle Switching States

        if (distanceFromTarget <= enemy.enemyType.strafingDistance && !enemy.enemyType.isAggresive)
        {
            return combatStanceState;
        }
        else if (distanceFromTarget <= enemy.enemyType.attackingDistance)
        {
            return attackState;
        }
        else
        {
            return this;
        }

        #endregion
    }

    private void RotateTowardsTarget()
    {   
        Vector3 targetDir = enemy.targetTransform.position - enemy.transform.position;
        targetDir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, enemy.enemyType.rotationSpeed * Time.deltaTime);
    }
}
