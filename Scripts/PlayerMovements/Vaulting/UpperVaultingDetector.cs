using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperVaultingDetector : MonoBehaviour
{
    [HideInInspector]
    public bool canVaultUpper;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canVaultUpper = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canVaultUpper = true;
        }
    }
}
