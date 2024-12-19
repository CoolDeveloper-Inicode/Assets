using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [Header("Sliding Properties")]
    public float slideSpeed;
    public float slidingResetTimer;
    public Transform playerObj;

    float slidingCooldown;
    float slideInputBuffer;

    //[HideInInspector]
    public bool isSliding;

    [Header("Scripts")]
    PlayerMovementTutorial playerMovementTutorial;
    Rigidbody rb;
    Animator anim;

    void Start()
    {
        playerMovementTutorial = GetComponent<PlayerMovementTutorial>();
        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (slidingCooldown > 0)
        {
            slidingCooldown -= Time.deltaTime;
        }

        if (slideInputBuffer > 0)
        {
            slideInputBuffer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && playerMovementTutorial.isSprinting || playerMovementTutorial.grounded && slideInputBuffer > 0)
        {
            //slide function
            Sliding();

            //set slide input buffer
            if (playerMovementTutorial.grounded)
                return;
    
            slideInputBuffer = 0.3f;
        }

        isSliding = anim.GetBool("isSliding");
    }

    void Sliding()
    {
        if (slidingCooldown > 0)
            return;

        if (!playerMovementTutorial.grounded)
            return;

        //set slide input buffer
        slideInputBuffer = 0;

        //set up animations
        anim.SetBool("isSliding", true);
        anim.Play("Slide");

        //add force to player when sliding
        rb.AddForce(playerObj.forward * slideSpeed * 10f, ForceMode.VelocityChange);

        //add a cooldown when sliding
        slidingCooldown = slidingResetTimer;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && isSliding)
        {
            collision.gameObject.GetComponentInChildren<Animator>().Play("TakeHeavyDamage");
        }
    }
}
