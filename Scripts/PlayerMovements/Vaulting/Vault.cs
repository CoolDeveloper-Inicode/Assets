using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    [Header("Vaulting Properties")]
    public Transform vaultingPosition;

    [Header("Scripts")]
    PlayerMovementTutorial playerMovementTutorial;
    Animator anim;

    LowerVaultingDetector lowerVaultingDetector;
    UpperVaultingDetector upperVaultingDetector;

    void Start()
    {
        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        anim = GetComponentInChildren<Animator>();

        lowerVaultingDetector = GetComponentInChildren<LowerVaultingDetector>();
        upperVaultingDetector = GetComponentInChildren<UpperVaultingDetector>();
    }

    void Update()
    {
        Vaulting();
    }

    void Vaulting()
    {
        if (playerMovementTutorial.grounded)
            return;

        if (lowerVaultingDetector.canVaultLower == true && upperVaultingDetector.canVaultUpper == true)
        {
            //plays vault animation
            anim.Play("Vault");

            //places player on the vaulting position
            StartCoroutine(LerpVault(vaultingPosition.position, 0.09f));

            //allowes player to double jump after vaulting
            playerMovementTutorial.canDoubleJump = true;
        }
    }

    IEnumerator LerpVault(Vector3 targetPos, float duration)
    {
        float time = 0f;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPos;
    }
}
