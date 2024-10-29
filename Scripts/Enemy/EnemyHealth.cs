using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public GameObject healthBar;
    public GameObject blood;
    public Transform bloodSpawn;
    public EnemyHealthUI enemyHealthUI;
    public BossHealthManager bossHealthManager;
    public EnemyStance enemyStance;

    float isTakingDamageTimer;
    float healthBarFadeTimer;

    float currentHealth;

    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool isInvincible;

    Animator anim;
    EnemyManager enemyManager;
    AttackState attackState;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        attackState = GetComponentInChildren<AttackState>();

        enemyManager = GetComponent<EnemyManager>();

        currentHealth = maxHealth;
        enemyHealthUI.SetMaxHealth(maxHealth);
        enemyHealthUI.SetCurrentHealth(currentHealth);
        healthBar.SetActive(false);
    }

    void Update()
    {
        #region Handle Health Bars

        if (!enemyManager.enemyType.isBoss)
        {
            if (healthBarFadeTimer > 0f)
            {
                healthBarFadeTimer -= Time.deltaTime;
            }
            else
            {
                healthBar.SetActive(false);
            }
        
        }

        #endregion

        #region Handle Death

        if (currentHealth <= 0)
        {
            anim.Play("Dead");
            isDead = true;

            if (bossHealthManager != null)
            {
                bossHealthManager.DeactivateBossHealthBar();
            }
        }

        #endregion

        #region Handle Damage Recovery

        if (isTakingDamageTimer > 0)
        {
            isTakingDamageTimer -= Time.deltaTime;
            anim.SetBool("isTakingDamage", true);

            anim.SetFloat("Vertical", 0f);
            anim.SetFloat("Horizontal", 0f);
        }
        else
        {
            anim.SetBool("isTakingDamage", false);
        }

        #endregion
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        if (isInvincible)
            return;

        currentHealth -= damage;
        enemyHealthUI.SetCurrentHealth(currentHealth);

        Invoke(nameof(SpawnEffects), 0.017f);

        #region Damage Animation

        healthBar.SetActive(true);
        healthBarFadeTimer = 3f;

        if (enemyStance != null)
        {
            if (enemyStance.heavyDamage)
            {
                isTakingDamageTimer = 0.9f;
                anim.Play("TakeHeavyDamage");
            }
            else
            {
                isTakingDamageTimer = 0.45f;
                enemyStance.enemyStanceAmount += 1f;
                    
                //randomly chooses a damage animation
                int damageDirection = Random.Range(1, 4);

                if (damageDirection == 1)
                {
                    anim.Play("TakeDamageRight");
                }
                else if (damageDirection == 2)
                {
                    anim.Play("TakeDamageLeft");
                }
                else if (damageDirection == 3)
                {
                    anim.Play("TakeDamage");
                }
            }
        }
        else
        {
            //randomly chooses a damage animation
            int damageDirection = Random.Range(1, 4);

            if (damageDirection == 1)
            {
                anim.Play("TakeDamageRight");
            }
            else if (damageDirection == 2)
            {
                anim.Play("TakeDamageLeft");
            }
            else if (damageDirection == 3)
            {
                anim.Play("TakeDamage");
            }
        }

        #endregion

        #region Getting Out Of Stun Lock

        //picks a number between 1 and 100
        float blockingOrDodgingChance = Random.Range(1, 100);

        if (blockingOrDodgingChance <= 25f)
        {
            attackState.Parry();
            attackState.Dodge();
        }

        #endregion
    }

    void SpawnEffects()
    {
        Instantiate(blood, bloodSpawn.position, bloodSpawn.rotation);
    }
}
