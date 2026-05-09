using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchState : State
{
    public AirState airState;
    public EnemyMovement enemyMovement;
    public Animator anim;

    public override State RunCurrentState()
    {
        anim.Play("LaunchUp");

        if (!enemyMovement.grounded)
        {
            return airState;
        }
        else
        {
            return this;
        }
    }
}
