using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    [Header("Vaulting Properties")]
    public Transform vaultingPosition;
    public float vaultingInputTime;

    [Header("Scripts")]
    PlayerMovementTutorial playerMovementTutorial;
    Animator anim;

    LowerVaultingDetector lowerVaultingDetector;
    UpperVaultingDetector upperVaultingDetector;

    float vaultingInputDelay;

    void Start()
    {
        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        anim = GetComponentInChildren<Animator>();

        lowerVaultingDetector = GetComponentInChildren<LowerVaultingDetector>();
        upperVaultingDetector = GetComponentInChildren<UpperVaultingDetector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vaultingInputDelay = vaultingInputTime;
        }

        if (vaultingInputDelay > 0f)
        {
            Vaulting();
            vaultingInputDelay -= Time.deltaTime;
        }
    }

    void Vaulting()
    {
        if (lowerVaultingDetector.canVaultLower == true && upperVaultingDetector.canVaultUpper == true)
        {
            if (Physics.Raycast(vaultingPosition.position, Vector3.down, out var hit))
            {
                Vector3 vaultPoint = hit.point;
                vaultPoint.y += 0.82f;

                //plays vault animation
                anim.Play("Vault");

                //places player on the vaulting position
                StartCoroutine(LerpVault(vaultPoint, 0.12f));

                //allowes player to double jump after vaulting
                playerMovementTutorial.canDoubleJump = true;
            }
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
