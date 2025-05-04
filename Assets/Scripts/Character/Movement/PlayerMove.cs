using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;

public class PlayerMove : MonoBehaviour
{
    #region Variables

    public CharacterController characterController;

    [Header("Walk")]
    public float defaultSpeed = 1f;
    [SerializeField] private float speed = 10f;
    public float gravityForce = 9.8f;
    public float defendingSpeed = 0.5f;


    [Header("Jump")]
    public float jumpForce = 15f;
    public float jumpTime = 0.5f;
    float time = 0;
    [SerializeField] bool canJump = true;
    public float coyoteTime = 0.1f;
    
    private float vSpeed = 0f;

    [Header("Dash")]
    public float DashMultiplier = 3f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.3f;
    public float dashStaminaCost = 15f;

    [Header("Sprint")]
    public float sprintMultiplier = 2f;
    public float sprintStaminaCost = 10f;
    private bool isSprinting = false;

    [SerializeField] bool canDash = true;
    [SerializeField] bool canRun = true;




    [Header ("References")]
    public GameObject playerSprite;
    public Animator animator;
    public new Transform camera;
    public GroundChecker groundCheck;
    public MeleeCombat meleeCombat;
    public StaminaScript stamina;



    [Header("Inputs")]

    public InputActionReference actionMove;
    public InputActionReference actionJump;
    public InputActionReference actionDash;
    public InputActionReference actionSprint;

    #endregion

    #region General
    // Update is called once per frame
    void Update()
    {
        if (meleeCombat != null)
        {
            if (!meleeCombat.IsAttacking() && !meleeCombat.IsDefending())
            {
                charJump();
                dash();
                charSprint();
                charWalk();

            }
        }
        else
        {
            charWalk();
            charJump();
            dash();
            charSprint();
            

        }

        if (isSprinting)
        {
            sprint();
        }

        gravity();
        
        if (groundCheck.IsGrounded())
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
    #endregion

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
        
        if (actionDash.action.triggered && canDash && groundCheck.IsGrounded() && stamina.UseStamina(dashStaminaCost))
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
            if (canRun && groundCheck.IsGrounded() )
            {
                    isSprinting = true;   
            }
        };

        actionSprint.action.canceled += ctx =>
        {
            sprintStop();
        };
    }


    public void sprint()
    {
        if (isSprinting && canRun && stamina.UseStamina(sprintStaminaCost*Time.deltaTime))
        {
            speed = defaultSpeed * sprintMultiplier;
            animator.SetBool("Sprint", true);
        }
        else
        {
            sprintStop();
        }
    }

    public void sprintStop()
    {
        isSprinting = false;
        speed = defaultSpeed;
        animator.SetBool("Sprint", false);
        canRun = false;
    }
    #endregion
}
