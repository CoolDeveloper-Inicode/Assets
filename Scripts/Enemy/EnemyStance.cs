using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStance : MonoBehaviour
{
    [HideInInspector]
    public bool heavyDamage;
    [HideInInspector]
    public float enemyStanceAmount;

    Animator anim;
    EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (enemyStanceAmount > 0f)
        {
            if (enemyStanceAmount <= 0f)
                return;
            
            enemyStanceAmount -= Time.deltaTime;
        }

        if (enemyStanceAmount >= 2f)
        {
            heavyDamage = true;
        }
        else
        {
            heavyDamage = false;
        }
    }
}
