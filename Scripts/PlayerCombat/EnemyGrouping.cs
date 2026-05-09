using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrouping : MonoBehaviour
{
    CameraMovement cameraMovement;

    void Start()
    {
        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    void Update()
    {
        if (cameraMovement.lockOnTargetTransform != null)
        {
            cameraMovement.lockOnTargetTransform.GetComponentInChildren<ChaseState>().chosenOne = true;
        }
    }
}
