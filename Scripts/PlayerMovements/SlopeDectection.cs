using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDectection : MonoBehaviour
{
    [Header("Slope Detection Properties")]
    public Transform slopeDectector;

    PlayerMovementTutorial playerMovementTutorial;
    Rigidbody rb;

    void Start ()
    {
        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        DectectForSlopes();
    }

    void DectectForSlopes()
    {
        if (playerMovementTutorial.OnSlope())
        {
            if (playerMovementTutorial.anim.GetBool("IsInteracting"))
            {
                RaycastHit hit;

                if (Physics.Raycast(slopeDectector.position, slopeDectector.forward, out hit, 1.4f, playerMovementTutorial.whatIsGround))
                    return;

                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
    }
}
