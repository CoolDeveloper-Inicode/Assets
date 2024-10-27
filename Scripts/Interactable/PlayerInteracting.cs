using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracting : MonoBehaviour
{
    [Header("Interacting Variables")]
    public GameObject interactingUI;
    public LayerMask interactingLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (var other in colliders)
            {
                if (other.CompareTag("Interactable"))
                {
                    interactingUI.SetActive(false);

                    IInteractable Iinteractable = other.GetComponent<IInteractable>();
                    Iinteractable.Interact();
                }
            }
        }

        if (Physics.CheckSphere(transform.position, 10f, interactingLayer))
        {
            interactingUI.SetActive(true);
        }
        else
        {
            interactingUI.SetActive(false);
        }
    }
}
