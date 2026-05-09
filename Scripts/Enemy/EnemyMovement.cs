using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float enemyHeight;
    public LayerMask groundLayer;

    [HideInInspector]
    public bool grounded;

    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, enemyHeight * 0.5f + 0.3f, groundLayer);

        if (grounded)
        {
            anim.SetBool("isInAir", false);
        }
    }
}
