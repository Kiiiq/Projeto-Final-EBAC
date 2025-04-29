using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

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
    public float DashMultiplier = 3f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.3f;

    [SerializeField] bool canDash = true;
    [SerializeField] bool canRun = true;




    [Header ("References")]
    public GameObject playerSprite;
    public Animator animator;
    public new Transform camera;
    public GroundChecker groundCheck;
    


    [Header("Inputs")]

    public InputActionReference actionMove;
    public InputActionReference actionJump;
    public InputActionReference actionDash;
    public InputActionReference actionSprint;



    // Update is called once per frame
    void Update()
    {
        
        charWalk();
        charJump();
        dash();
        charSprint();
        
        
        gravity();
        if ( groundCheck.IsGrounded())
        {
            canJump = true;
            canRun = true;
            animator.SetBool("Land", true);
        }

    }


    public void gravity()
    {
        if (!groundCheck.IsGrounded())
        {

            vSpeed -= gravityForce * Time.deltaTime;

        }

        characterController.Move(new Vector3(0, vSpeed * Time.deltaTime, 0));

    }

    #region Walk
    public void charWalk()
    {

        var playerInput = actionMove.action.ReadValue<Vector2>();
        //var inputAxisHorizontal = Input.GetAxis("Horizontal");

        Vector3 forward = camera.TransformDirection(Vector3.forward);
        Vector3 right = camera.TransformDirection(Vector3.right);

        Vector3 ForwardRelative = new Vector3(forward.x, 0, forward.z)* playerInput.y;
        Vector3 RightRelative = new Vector3(right.x, 0, right.z) * playerInput.x;


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

        if (actionJump.action.IsPressed() && canJump) {
            time = Time.time;
            canJump = false;
            animator.SetBool("Jump", true);
            animator.SetBool("Land", false);
        }
       float timeleft = (Time.time - time);
        if (actionJump.action.IsPressed() && jumpTime>timeleft)
        {
            vSpeed = jumpForce;
            characterController.Move(new Vector3(0, jumpTime * jumpForce * Time.deltaTime, 0));
        } else
        {
            animator.SetBool("Jump", false);
        }
            coyotteJump();
    }

    public void coyotteJump()
    {
        float timetojump;
        if (!groundCheck.IsGrounded())
        {
            
            timetojump = Time.time;
            if (Time.time - time > coyoteTime)
            {
                //canDash = false;
                canJump = false;
                canRun = false;
            }
        }




    }
    #endregion

    #region Dash

    public void dash()
    {
        
        if (actionDash.action.triggered && canDash && groundCheck.IsGrounded())
        {
            StartCoroutine(CharDash());
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

    #endregion

    #region Sprint

   
    public void charSprint()
    {
        actionSprint.action.performed += ctx =>
        {
            if (canRun && groundCheck.IsGrounded())
            {
                speed = sprintMultiplier * defaultSpeed;
                animator.SetBool("Sprint", true);
            }
        };

        actionSprint.action.canceled += ctx =>
        {
            speed = defaultSpeed;
            animator.SetBool("Sprint", false);
            canRun = false;
        };
    }
    #endregion
}
