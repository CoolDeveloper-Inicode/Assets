using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour, IInteractable
{
    [Header("Properties")]
    public int numOfPotions;

    PlayerHealing playerHealing;

    void Start()
    {
        playerHealing = FindObjectOfType<PlayerHealing>();
    }

    public void Interact()
    {
        playerHealing.amountOfPotions += numOfPotions;
        Destroy(gameObject);
    }
}
