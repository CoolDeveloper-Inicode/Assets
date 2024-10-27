using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public Transform lockOnDetector;

    CameraMovement cameraMovement;

    void Start ()
    {
        cameraMovement = GetComponentInChildren<CameraMovement>();
    }

    void Update ()
    {
        Collider[] colliders = Physics.OverlapSphere(lockOnDetector.position, 10f);

        foreach (var other in colliders)
        {
            if (other.CompareTag("Enemy"))
            {
                if (cameraMovement.lockOnFlag && !cameraMovement.lockOnTargetTransform.GetComponent<EnemyHealth>().isDead)
                    return;
                
                Transform targetTransform = other.GetComponent<Transform>();
                cameraMovement.lockOnTargetTransform = targetTransform;

                if (other.GetComponent<EnemyHealth>().isDead)
                {
                    cameraMovement.lockOnFlag = false;
                    cameraMovement.lockOnTargetTransform.gameObject.tag = "Untagged";
                    cameraMovement.lockOnTargetTransform = null;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(lockOnDetector.position, 10f);
    }
}
