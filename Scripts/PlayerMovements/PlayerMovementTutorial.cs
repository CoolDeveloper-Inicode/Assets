using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float slidingSpeed;

    public float speedMultiplier;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Audios")]
    public AudioSource audioSource;

    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    float speed;
    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;

    [HideInInspector]
    public bool isSprinting;

    Vector3 moveDirection;

    Dodge dodge;
    Rigidbody rb;
    CameraMovement cameraMovement;
    Slide slide;

    [HideInInspector]
    public bool canDoubleJump;
    bool isDoubleJumping;

    float doubleJumpTimer;

    [HideInInspector]
    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dodge = GetComponent<Dodge>();
        slide = GetComponent<Slide>();
        cameraMovement = GetComponentInChildren<CameraMovement>();
        anim = GetComponentInChildren<Animator>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        if (anim.GetBool("IsInteracting"))
            isSprinting = false;

        #region Ground Checks

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        // handle drag
        if (grounded)
        {
            anim.SetBool("IsJumping", false);
            rb.drag = groundDrag;
        }
        else
        {
            anim.SetBool("IsJumping", true);
            rb.drag = 0;
        }

        #endregion

        if (!anim.GetBool("IsInteracting"))
        {
            MyInput();
            SpeedControl();

            #region Handle Movement Animations

            if (horizontalInput == 0 && verticalInput == 0)
            {
                isSprinting = false;

                anim.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
            }
            else
            {
                if (slide.isSliding)
                {
                    desiredMoveSpeed = slidingSpeed;
                }
                else if (dodge.canSprint)
                {
                    anim.SetFloat("Vertical", 2f, 0.2f, Time.deltaTime);
                    anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
                    desiredMoveSpeed = sprintSpeed;
                    isSprinting = true;
                }
                else if (!dodge.canSprint)
                {
                    desiredMoveSpeed = walkSpeed;
                    isSprinting = false;

                    if (cameraMovement.lockOnFlag)
                    {
                        if (isSprinting)
                            return;

                        if (horizontalInput == 0 && verticalInput == 0)
                        {
                            anim.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                            anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
                        }
                        else if (horizontalInput > 0)
                        {
                            anim.SetFloat("Vertical", 1f, 0.2f, Time.deltaTime);
                            anim.SetFloat("Horizontal", -1f, 0.2f, Time.deltaTime);
                        }
                        else if (horizontalInput < 0)
                        {
                            anim.SetFloat("Vertical", 1f, 0.2f, Time.deltaTime);
                            anim.SetFloat("Horizontal", 1f, 0.2f, Time.deltaTime);
                        }
                        else if (verticalInput > 0)
                        {
                            anim.SetFloat("Vertical", 1f, 0.2f, Time.deltaTime);
                            anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
                        }
                        else if (verticalInput < 0)
                        {
                            anim.SetFloat("Vertical", -1f, 0.2f, Time.deltaTime);
                            anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
                        }
                    }
                    else
                    {
                        anim.SetFloat("Vertical", 1f, 0.2f, Time.deltaTime);
                        anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
                    }
                }
            }

            #endregion
        }
        else
        {
            audioSource.enabled = false;
        }

        if (doubleJumpTimer > 0f)
        {
            doubleJumpTimer -= Time.deltaTime;
        }
        else
        {
            isDoubleJumping = false;
        }

        if (Mathf.Abs(lastDesiredMoveSpeed - desiredMoveSpeed) > 4f && speed != 0f)
        {
            StopCoroutine("SmoothlyLerpMoveSpeed");
            StartCoroutine("SmoothlyLerpMoveSpeed");
        }
        else
        {
            speed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (anim.GetBool("IsInteracting"))
            return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            if (!isDoubleJumping)
            {
                anim.SetTrigger("TakeOf");
            }

            readyToJump = false;
            StartCoroutine(TakeOf());
            canDoubleJump = true;
        }

        #region Handle Double Jumping

        if(Input.GetKeyDown(jumpKey) && !grounded && canDoubleJump)
        {
            anim.CrossFade("DoubleJump", 0.1f);

            if (grounded)
                return;

            isDoubleJumping = true;
            canDoubleJump = false;
            readyToJump = false;
            StartCoroutine(TakeOf());
            doubleJumpTimer = 0.26f;
        }

        #endregion

        #region Handle Sounds

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (grounded)
            {
                audioSource.enabled = true;

                if (isSprinting)
                {
                    audioSource.pitch = 1.6f;
                }
                else
                {
                    audioSource.pitch = 1f;
                }
            }
            else
            {
                audioSource.enabled = false;
            }
        }
        else
        {
            audioSource.enabled = false;
        }

        #endregion
    }

    private void MovePlayer()
    {
        if (anim.GetBool("IsInteracting"))
            return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * speed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

        //turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (anim.GetBool("IsInteracting"))
            return;

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > speed)
                rb.velocity = rb.velocity.normalized * speed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if(flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    IEnumerator TakeOf()
    {
        yield return new WaitForSeconds(0.09f);

        Jump();

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - speed);
        float startValue = speed;

        while (time < difference)
        {
            speed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (desiredMoveSpeed > speed)
            {
                time += Time.deltaTime * speedMultiplier;
            }
            else
            {
                time += Time.deltaTime;
            }

            yield return null;
        }

        speed = desiredMoveSpeed;
    }
}