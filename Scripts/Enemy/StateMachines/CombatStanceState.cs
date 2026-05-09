using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public EnemyManager enemy;
    public Animator anim;
    public AttackState attackState;
    public ChaseState chaseState;
    public EnemyHealth enemyHealth;
    public DeadState deadState;
    public EnemyMovement enemyMovement;
    public AirState airState;

    [HideInInspector]
    public bool rollForStrafeChance;

    float horizontalMovement;

    public override State RunCurrentState()
    {
        if (enemyHealth.isDead || enemyHealth.currentHealth <= 0f)
            return deadState;

        if (!enemyMovement.grounded)
            return airState;

        //determines the distance from the player
        float distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.targetTransform.position);

        //rotate to look at target
        RotateTowardsTarget();

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

        #region Handle Switching States

        if (distanceFromTarget <= enemy.enemyType.attackingDistance)
        {
            return attackState;
        }
        else if (distanceFromTarget > enemy.enemyType.strafingDistance)
        {
            return chaseState;
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
