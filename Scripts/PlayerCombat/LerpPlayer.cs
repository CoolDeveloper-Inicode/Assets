using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LerpPlayer : MonoBehaviour
{
    CameraMovement cameraMovement;
    PlayerCombatSystem playerCombatSystem;
    Dodge dodge;

    void Start()
    {
        playerCombatSystem = GetComponent<PlayerCombatSystem>();
        dodge = GetComponent<Dodge>();

        cameraMovement = GetComponentInChildren<CameraMovement>();
    }

    void Update()
    {
        if (cameraMovement.lockOnTargetTransform != null)
        {
            if (cameraMovement.lockOnTargetTransform.GetComponent<EnemyHealth>().isInvincible)
                return;

            if (playerCombatSystem.canTp && Vector3.Distance(transform.position, cameraMovement.lockOnTargetTransform.position) <= 8f 
                && Vector3.Distance(transform.position, cameraMovement.lockOnTargetTransform.position) > 0.7f)
            {
                transform.DOMove(TargetOffset(cameraMovement.lockOnTargetTransform), 0.2f);

                //rotates the player to the enemy
                if (!cameraMovement.lockOnFlag)
                {
                    Vector3 targetPosition = cameraMovement.lockOnTargetTransform.position - dodge.playerObj.position;
                    targetPosition.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

                    dodge.playerObj.rotation = Quaternion.Slerp(dodge.playerObj.rotation, targetRotation, 1f);
                }
            }
        }
    }

    private Vector3 TargetOffset(Transform enemy)
    {
        return Vector3.MoveTowards(enemy.position, transform.position, 1f);
    }
}
