using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController characterController;

    [Header("Walk")]
    public float defaultSpeed = 1f;
    [SerializeField] private float speed = 10f;
    public float gravityForce = 9.8f;


    [Header("Jump")]
    public float jumpForce = 15f;
    public float jumpTime = 0.5f;
    float time = 0;
    [SerializeField] bool canJump = true;
    public float coyoteTime = 0.1f;
    
    private float vSpeed = 0f;

    [Header("Dash/Sprint")]
    public float sprintMultiplier = 2f;
    public float DashMultiplier = 5f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.5f;

    private float pressingTime = 0f;
    [SerializeField] bool canDash = true;
    [SerializeField] bool canRun = true;




    [Header ("References")]
    public GameObject playerSprite;
    public Animator animator;
    public Transform camera;


    [Header("KeyCodes")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;



    // Update is called once per frame
    void Update()
    {
        charWalk();
        charJump();
        dashOrSprint();
        if ( characterController.isGrounded)
        {
            canJump = true;
            
            canRun = true;
            animator.SetBool("Jumping", false);
        }

    }

    #region Walk
    public void charWalk()
    {

        var inputAxisVertical = Input.GetAxis("Vertical");
        var inputAxisHorizontal = Input.GetAxis("Horizontal");

        Vector3 forward = camera.TransformDirection(Vector3.forward);
        Vector3 right = camera.TransformDirection(Vector3.right);

        Vector3 ForwardRelative = new Vector3(forward.x, 0, forward.z)*inputAxisVertical;
        Vector3 RightRelative = new Vector3(right.x, 0, right.z) * inputAxisHorizontal;


        Vector3 moveDirection = ForwardRelative+RightRelative;

        characterController.Move(speed * Time.deltaTime * moveDirection);
        if (moveDirection != Vector3.zero)
        {
            charRotate(moveDirection);
            animator.SetBool("Run", true);
        } else {
            animator.SetBool("Run", false);
        }
        
    }

    public void charRotate(Vector3 vector3) { 
        playerSprite.transform.forward = vector3;
    }
    #endregion


    #region Jump
    public void charJump()
    {
        float jumpspeed;

        

        if (Input.GetKeyDown(jumpKey) && canJump) {
            time = Time.time;
            canJump = false;
            animator.SetBool("Jumping", true);
        }
       float timeleft = (Time.time - time);
        if (Input.GetKey(jumpKey)&& jumpTime>timeleft)
        {
            vSpeed = jumpForce;
            characterController.Move(new Vector3(0, jumpTime* jumpForce * Time.deltaTime, 0));
        } 
            gravity();
            coyotteJump();


    }

    public void gravity()
    {
        if (!characterController.isGrounded)
        {
            if (canJump) { vSpeed = 0; }
            
            vSpeed -= gravityForce * Time.deltaTime;
            characterController.Move(new Vector3(0, vSpeed * Time.deltaTime, 0));
        }
    }

    public void coyotteJump()
    {
        float timetojump;
        if (!characterController.isGrounded)
        {
            timetojump = Time.time;
            if (Time.time - time > coyoteTime)
            {
                canJump = false;
                canRun = false;
            }
        }




    }
    #endregion


    #region Dash/Sprint

    public void dashOrSprint()
    {

        if (Input.GetKeyDown(runKey) && canDash)
        {
            Debug.Log("Pressed");
            pressingTime = Time.time;
        }
        float timeSincePressed = Time.time - pressingTime;

        //Debug.Log(timeSincePressed);

        if (Input.GetKeyUp(runKey) && Time.time-pressingTime <1 && canDash)
        {
            Debug.Log("Dash");
            StartCoroutine(CharDash());
        } else if (Input.GetKey(runKey) && canRun)
        {
            charSprint();
        }
    }

    IEnumerator CharDash()
    {
        canDash = false;
        canJump = false;
        canRun = false;
        animator.SetBool("Dash", true);
        speed = defaultSpeed*DashMultiplier;
        yield return new WaitForSeconds(dashTime);
        speed = defaultSpeed;
        animator.SetBool("Dash", false);
        yield return new WaitForSeconds(dashCooldown-dashTime);
        canDash = true;
    }

    public void charSprint()
    {
        if (Input.GetKey(runKey))
        {
            
            speed = sprintMultiplier*defaultSpeed;
        }
        if (Input.GetKeyUp(runKey))
        {
            speed = defaultSpeed;
            canRun = false;
        }
    }

    #endregion
}
