using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerVaultingDetector : MonoBehaviour
{
    [HideInInspector]
    public bool canVaultLower;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canVaultLower = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canVaultLower = false;
        }
    }
}
