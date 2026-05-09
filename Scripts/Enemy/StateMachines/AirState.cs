using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State
{
    public ChaseState chaseState;
    public EnemyMovement enemyMovement;
    public EnemyManager enemy;
    public EnemyHealth enemyHealth;

    public override State RunCurrentState()
    {
        enemyHealth.isLaunched = false;

        //rotates enemy towards target
        RotateTowardsTarget();

        if (enemyMovement.grounded)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDir = enemy.targetTransform.position - enemy.transform.position;
        targetDir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, enemy.enemyType.rotationSpeed * Time.deltaTime);
    }
}
