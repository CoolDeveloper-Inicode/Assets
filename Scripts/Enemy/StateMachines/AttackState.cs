using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public Animator anim;
    public EnemyManager enemy;
    public CombatStanceState combatStanceState;
    public EnemyHealth enemyHealth;
    public DeadState deadState;
    public PlayerCombatSystem playerCombatSystem;
    public EnemyMovement enemyMovement;
    public AirState airState;
    public LaunchState launchState;

    [HideInInspector]
    public bool canParry;

    bool rollForDodgeChance;
    bool rollForParryChance;

    bool hasPerformedDodge;

    public override State RunCurrentState()
    {
        if (enemyHealth.isDead || enemyHealth.currentHealth <= 0f)
            return deadState;

        if (enemyHealth.isLaunched)
            return launchState;

        if (!enemyMovement.grounded)
            return airState;

        //determines the distance from the target
        float distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.targetTransform.position);

        //rotates enemy towards target
        RotateTowardsTarget();

        #region Handle Dodging

        if (playerCombatSystem.isAttacking && !rollForDodgeChance && !hasPerformedDodge)
        {
            Dodge();
            rollForDodgeChance = true;
        }

        #endregion

        #region Handle Parrying

        if (playerCombatSystem.isAttacking && !rollForParryChance && !canParry)
        {
            Parry();
            rollForParryChance = true;
        }

        #endregion

        #region Handle Switching States

        if (distanceFromTarget > enemy.enemyType.attackingDistance && !hasPerformedDodge)
        {
            return combatStanceState;
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

    #region Dodging Functions

    public void Dodge()
    {
        if (enemyHealth.isDead || enemyHealth.currentHealth <= 0f)
            return;

        if (!enemyMovement.grounded)
            return;

        float dodgeChance = Random.Range(1, 100);

        int dodgeDirection = Random.Range(1, 4);

        if (dodgeChance <= enemy.enemyType.dodgingLikelyHood)
        {
            if (dodgeDirection == 1)
            {
                anim.Play("DodgeRight");
                enemy.rb.AddForce(enemy.transform.right * 40f, ForceMode.Impulse);
            }
            else if (dodgeDirection == 2)
            {
                anim.Play("DodgeLeft");
                enemy.rb.AddForce(-enemy.transform.right * 40f, ForceMode.Impulse);
            }
            else if (dodgeDirection == 3)
            {
                anim.Play("DodgeBack");
                enemy.rb.AddForce(-enemy.transform.forward * 40f, ForceMode.Impulse);
            }

            hasPerformedDodge = true;
            enemyHealth.isInvincible = true;
            StartCoroutine(ResetDodge());
        }
    }

    IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(0.35f);

        rollForDodgeChance = false;
        hasPerformedDodge = false;
        enemyHealth.isInvincible = false;
    }

    #endregion

    #region Parrying Functions

    public void Parry()
    {
        if (enemyHealth.isDead || enemyHealth.currentHealth <= 0f)
            return;

        if (!enemyMovement.grounded)
            return;

        float parryChance = Random.Range(1, 100);

        if (parryChance <= enemy.enemyType.parryLikelyHood)
        {
            canParry = true;
            StartCoroutine(ResetParry());
        }
    }

    IEnumerator ResetParry()
    {
        yield return new WaitForSeconds(0.25f);

        canParry = false;
        rollForParryChance = false;
    }

    #endregion
}
