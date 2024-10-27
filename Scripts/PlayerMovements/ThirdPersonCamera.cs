using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float rotationSpeed;

    public Transform orien;
    public Transform playerObj;
    public Transform player;

    public Animator anim;

    PlayerMovementTutorial playerMovementTutorial;
    CameraMovement cameraMovement;
    Dodge dodge;

    void Start()
    {
        playerMovementTutorial = GetComponentInParent<PlayerMovementTutorial>();
        cameraMovement = GetComponentInParent<CameraMovement>();
        dodge = GetComponentInParent<Dodge>();
    }

    void Update()
    {
        if (dodge.isDodging)
            return;

        if (!cameraMovement.lockOnFlag || playerMovementTutorial.isSprinting)
        {
            if (anim.GetBool("IsInteracting"))
                return;

            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orien.forward = viewDir.normalized;

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = orien.forward * verticalInput + orien.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 targetPosition = cameraMovement.lockOnTargetTransform.position - transform.position;
            targetPosition.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRotation, 25f * Time.deltaTime);
            orien.rotation = Quaternion.Slerp(orien.rotation, targetRotation, 25f * Time.deltaTime);
        }
    }
}
