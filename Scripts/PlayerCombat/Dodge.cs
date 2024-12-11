using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [Header("Dodging Properties")]
    public float dodgeSpeed;
    public Transform orientation;
    public Transform playerObj;

    [HideInInspector]
    public bool canSprint;
    [HideInInspector]
    public bool isDodging;
    [HideInInspector]
    public bool isInvincible;

    float leftShiftInputTimer;

    float verticalInput;
    float horizontalInput;

    PlayerAnimatorController playerAnimatorController;
    PlayerMovementTutorial playerMovementTutorial;
    PlayerCombatSystem playerCombatSystem;
    Rigidbody rb;
    CharacterMeshTrail characterMeshTrail;

    void Start()
    {
        playerAnimatorController = GetComponentInChildren<PlayerAnimatorController>();

        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        rb = GetComponent<Rigidbody>();
        playerCombatSystem = GetComponent<PlayerCombatSystem>();
        characterMeshTrail = GetComponent<CharacterMeshTrail>();
    }

    void Update()
    {
        if (playerCombatSystem.isAttacking)
            return;

        if (isDodging)
            return;

        if (!playerMovementTutorial.grounded)
            return;

        #region Handle Whether If Player Is Dodging Or Sprinting

        if (Input.GetKey(KeyCode.LeftShift))
        {
            canSprint = true;
            leftShiftInputTimer += Time.deltaTime;
        }
        else
        {
            canSprint = false;

            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");

            if (leftShiftInputTimer > 0f && leftShiftInputTimer < 0.5f)
            {
                if (horizontalInput != 0 || verticalInput != 0)
                {
                    Vector3 dir = orientation.forward * verticalInput + orientation.right * horizontalInput;

                    playerAnimatorController.PlayTargetAnimation("Dodge", true);
                    Invoke(nameof(SpawnTrail), 0.07f);

                    if (!playerMovementTutorial.OnSlope())
                    {
                        rb.AddForce(dir * dodgeSpeed * 10f, ForceMode.Impulse);
                    }
                    else
                    {
                        rb.AddForce(playerMovementTutorial.GetSlopeMoveDirection() * dodgeSpeed * 10f, ForceMode.Impulse);
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRotation, 0.8f);
                    StartCoroutine(ResetDodge());
                    StartCoroutine(ResetInvincibility());
                }
            }

            leftShiftInputTimer = 0f;
        }

        #endregion
    }

    #region Handle Coroutines

    IEnumerator ResetDodge()
    {
        isDodging = true;

        yield return new WaitForSeconds(0.225f);

        isDodging = false;
    }

    IEnumerator ResetInvincibility()
    {
        isInvincible = true;

        yield return new WaitForSeconds(0.5f);

        isInvincible = false;
    }

    #endregion

    void SpawnTrail()
    {
        if (!characterMeshTrail.isTrailActive)
        {
            characterMeshTrail.isTrailActive = true;
            StartCoroutine(characterMeshTrail.ActivateTrail(characterMeshTrail.activeTime));
        }
    }
}
