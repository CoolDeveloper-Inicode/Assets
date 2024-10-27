using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepMovement : MonoBehaviour
{
    [Header("Step Movement Variables")]
    public GameObject lowerStepDetector;
    public GameObject upperStepDetector;
    public float stepSmooth;
    public LayerMask layersToStepInto;
    public Transform playerObj;
    
    Rigidbody rb;
    PlayerMovementTutorial playerMovementTutorial;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
    }

    private void Update()
    {
        if (playerMovementTutorial.OnSlope())
            return;

        ClimpSteps();
    }

    private void ClimpSteps()
    {
        RaycastHit hitLower;

        if (Physics.Raycast(lowerStepDetector.transform.position, playerObj.forward, out hitLower, 0.3f, layersToStepInto))
        {
            RaycastHit hitUpper;

            if (!Physics.Raycast(upperStepDetector.transform.position, playerObj.forward, out hitUpper, 0.3f, layersToStepInto))
            {
                rb.position -= new Vector3(0, -stepSmooth, 0);
            }
        }
    }
}
