using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArtUI : MonoBehaviour
{
    [Header("Skill Art UI Properties")]
    public float maxSkillAmount;

    public PlayerUI playerUI;

    [HideInInspector]
    public float currentSkillAmount;

    void Start()
    {
        currentSkillAmount = maxSkillAmount;

        playerUI.SetMaxHealth(maxSkillAmount);
        playerUI.SetCurrentHealth(currentSkillAmount);
    }

    void Update()
    {
        if (currentSkillAmount < maxSkillAmount)
        {
            currentSkillAmount += Time.deltaTime;
            playerUI.SetCurrentHealth(currentSkillAmount);
        }
    }
}
