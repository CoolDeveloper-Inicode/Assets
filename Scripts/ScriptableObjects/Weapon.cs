using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    public float lightAttackDamage;

    public float heavyAttackDamage;

    public float skillAttackDamage;

    [HideInInspector]
    public float damage;

    public GameObject prefab;

    public List<string> lightAttackAnimations;

    public List<string> lightAttackAirAnimations;

    public List<string> heavyAttackAnimations;

    public List<string> heavyAttackAirAnimations;

    public string skillAttackAnimation;

    public string launcherAttackAnimation;

    public string groundSlamAnimation;
}
