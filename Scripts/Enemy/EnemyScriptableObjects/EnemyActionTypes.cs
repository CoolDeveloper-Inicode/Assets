using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttacks", menuName = "Enemy/Create New Enemy Attack")]
public class EnemyActionTypes : ScriptableObject
{
    public List<string> enemyAttacks;
}
