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
    float airHitTimer;

    [HideInInspector]
    public float currentHealth;

    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool isInvincible;
    [HideInInspector]
    public float parryOrDodgeChance;
    [HideInInspector]
    public bool isLaunched;

    Animator anim;
    Animator playerAnim;
    PlayerAnimatorController playerAnimController;
    GameObject player;
    EnemyManager enemyManager;
    AttackState attackState;
    ChaseState chaseState;
    Rigidbody rb;
    EnemyMovement enemyMovement;
    AirState airState;
    StateManager state;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        attackState = GetComponentInChildren<AttackState>();
        chaseState = GetComponentInChildren<ChaseState>();
        airState = GetComponentInChildren<AirState>();
        player = GameObject.Find("Samurai");
        playerAnim = player.GetComponentInChildren<Animator>();
        playerAnimController = player.GetComponentInChildren<PlayerAnimatorController>();

        enemyManager = GetComponent<EnemyManager>();
        rb = GetComponent<Rigidbody>();
        enemyMovement = GetComponent<EnemyMovement>();
        state = GetComponent<StateManager>();

        currentHealth = maxHealth;
        enemyHealthUI.SetMaxHealth(maxHealth);
        enemyHealthUI.SetCurrentHealth(currentHealth);
        healthBar.SetActive(false);

        parryOrDodgeChance = 25f;
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
            if (!playerAnim.GetBool("isSkillAttack"))
            {
                anim.Play("Dead");
                healthBar.SetActive(false);
                isDead = true;

                if (bossHealthManager != null)
                {
                    bossHealthManager.DeactivateBossHealthBar();
                }
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

        if (airHitTimer > 0f)
        {
            airHitTimer -= Time.deltaTime;
        }
        else
        {
            if (airHitTimer <= 0f)
            {
                anim.SetBool("isAirHit", false);
            }
        }

        #endregion
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        if (isInvincible)
            return;

        #region Getting Out Of Stun Lock

        if (enemyMovement.grounded)
        {
            if (currentHealth > 0f)
            {
                //picks a number between 1 and 100
                float blockingOrDodgingChance = Random.Range(1, 100);

                if (blockingOrDodgingChance <= parryOrDodgeChance)
                {
                    attackState.Parry();
                    attackState.Dodge();
                }
            }
        }

        #endregion

        currentHealth -= damage;
        enemyHealthUI.SetCurrentHealth(currentHealth);

        Invoke(nameof(SpawnEffects), 0.017f);

        if (currentHealth > 0f)
        { 
            healthBar.SetActive(true);
            healthBarFadeTimer = 3f;
        }
        else
        {
            healthBarFadeTimer = 0f;
        }

        #region Damage Animation

        if (enemyMovement.grounded)
        {
            if (!playerAnimController.canLaunchUp)
            {
                if (enemyStance != null)
                {
                    if (enemyStance.heavyDamage)
                    {
                        isTakingDamageTimer = 0.9f;
                        anim.Play("TakeHeavyDamage");
                        enemyStance.enemyStanceAmount = 0f;
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
            }
            else
            {
                isLaunched = true;
                rb.AddForce(enemyManager.transform.up * 30f, ForceMode.Impulse);
            }
        }
        else
        {
            anim.Play("AirHit");

            if (playerAnim.GetBool("isGroundSlam"))
            {
                anim.SetBool("isAirHit", false);
                rb.AddForce(-enemyManager.transform.up * 35f, ForceMode.Impulse);
                anim.Play("LandDown");
            }
            else
            {
                anim.SetBool("isAirHit", true);
                airHitTimer = 1f;
            }
        }

        #endregion
    }

    void SpawnEffects()
    {
        Instantiate(blood, bloodSpawn.position, bloodSpawn.rotation);
    }
}
