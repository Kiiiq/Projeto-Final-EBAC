using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Windows.Speech;

public class PlayerStateManager : MonoBehaviour
{
    #region Attributes
    #region References

    [Header("References")]
    public CharacterController characterController;
    PlayerInputs input;
    public GameObject playerSprite;
    public Animator animator;
    public new Transform playerCamera;
    public GroundChecker groundCheck;
    public StaminaScript stamina;
    public CorroutineHandler corroutineHandler;
    public HealthManager healthManager;

    [SerializeField] public CinemachineVirtualCamera combatCamera;
    [SerializeField] public CinemachineFreeLook explorationCamera;
    [SerializeField] public CameraScript cameraScript;

    [SerializeField] public GameObject player;

    [SerializeField] public List<WeaponSO> Weapons = new List<WeaponSO>();
    [SerializeField] private GameObject parentObject;
    [SerializeField] public WeaponSO currentWeapon;
    [SerializeField]private int currentIndex = 0;

    #endregion

    #region State Variables
    [Header("State")]
    [SerializeField] PlayerStateBase currentState;
    [SerializeField] PlayerStateFactory states;
    #endregion 

    #region Key Checkers
    [Header("Checkers")]
    [SerializeField] bool _WalkButtonPressed;
    [SerializeField] bool _JumpButtonPressed;
    [SerializeField] bool _DashButtonPressed;
    [SerializeField] bool _SprintButtonPressed;
    [SerializeField] bool _AttackButtonPressed;
    [SerializeField] bool _DefenseButtonPressed;
    [SerializeField] bool _SwitchToSwordButtonPressed;
    [SerializeField] bool _SwitchToDaggerButtonPressed;
    

    #region Getters/Setters 
    public PlayerStateBase CurrentState { get => currentState; set => currentState = value; }
    public bool WalkButtonPressed { get => _WalkButtonPressed; set => _WalkButtonPressed = value; }
    public bool JumpButtonPressed { get => _JumpButtonPressed; set => _JumpButtonPressed = value; }
    public bool DashButtonPressed { get => _DashButtonPressed; set => _DashButtonPressed = value; }
    public bool SprintButtonPressed { get => _SprintButtonPressed; set => _SprintButtonPressed = value; }
    public bool AttackButtonPressed { get => _AttackButtonPressed; set => _AttackButtonPressed = value; }
    public bool DefenseButtonPressed { get => _DefenseButtonPressed; set => _DefenseButtonPressed = value; }
    public bool SwitchToSwordButtonPressed { get => _SwitchToSwordButtonPressed; set => _SwitchToSwordButtonPressed = value; }
    public bool SwitchToDaggerButtonPressed { get => _SwitchToDaggerButtonPressed; set => _SwitchToDaggerButtonPressed = value; }

    #endregion
    #endregion

    #region Walk
    [Header("Walk")]
    [SerializeField]const float defaultSpeed = 10f;
    float speed = 10f;
    public float gravityForce = 9.8f;
    public float defendingSpeed = 0.5f;
    private bool walkButtonPressed = false;
    public Vector2 moveInput;

    [Header("Sprint")]
    public float sprintMultiplier = 2f;
    public float sprintStaminaCost = 10f;
    private bool isSprinting = false;
    [SerializeField] bool canRun = true;


    #region Getters/Setters
    public float DefaultSpeed { get => defaultSpeed; }
            public float Speed { get => speed;  set => speed = value; }
    #endregion
    #endregion

    #region Jump
    [Header("Jump")]
    public float jumpForce = 15f;
    public float jumpTime = 0.5f;
    [SerializeField] bool canJump = true;
    [SerializeField] bool isJumping = false;

    private float vSpeed = 0f;

        #region Getters/Setters
            public float VSpeed { get => vSpeed; set => vSpeed = value; }
            public bool CanJump { get => canJump; set => canJump = value; }
            public bool IsJumping { get => isJumping; set => isJumping = value; }
    #endregion
    #endregion
    
    #region Dash

    [Header("Dash")]
    
    [SerializeField] float dashMultiplier = 3f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float dashCooldown = 0.3f;
    [SerializeField] float dashStaminaCost = 15f;
    [SerializeField] bool isDashing;
    [SerializeField] bool canDash = true;
    [SerializeField] Vector3 lastMoveDir;


    #region Getters/Setters
    public float DashMultiplier { get => dashMultiplier;}
    public float DashTime { get => dashTime;}
    public float DashCooldown { get => dashCooldown;}
    public float DashStaminaCost { get => dashStaminaCost;}
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool CanDash { get => canDash; set => canDash = value; }

    public Vector3 LastMoveDir { get => lastMoveDir; set => lastMoveDir = value; }

    #endregion

    #endregion

    #region Combat


    public bool focused = false;

    //[Header("Sword Attack")]

    //public float swordCooldownTime;
    //public float swordStaminaCost = 23f;
    //public float swordDamage;
    //public float swordAttackDuration;

    //[Header("Dagger Attack")]

    //public float daggerCooldownTime;
    //public float daggerStaminaCost = 12f;
    //public float daggerDamage;
    //public float daggerAttackDuration;

    public bool CanAttack = true;

    [Header("Defense")]
    
    private float defenseStaminaCost = 1f; // stamina cost per 1 damage blocked
    public float damageReduction = 0.5f;
    private float defenseSlow = 0.5f;


    private float actualStaminaCost;
    private bool sword = true;
    [SerializeField] private bool attacking;
    private bool defending;

    #region Getters/Setters
    public bool Attacking { get => attacking; set => attacking = value; }
    public bool Defending { get => defending; set => defending = value; }
    public float DefenseSlow { get => defenseSlow; set => defenseSlow = value; }
    public bool Sword { get => sword; set => sword = value; }

    public float DefenseStaminaCost { get => defenseStaminaCost; set => defenseStaminaCost = value; }

    #endregion

    #endregion
    #endregion



    private void Awake()
    {
        input = new PlayerInputs();

        foreach (WeaponSO i in Weapons)
        {
            i.weapon=Instantiate(i.weaponPrefab, parentObject.transform);
            i.weapon.GetComponent<WeaponScript>().weaponData = i;
            i.weapon.SetActive(false);
        }

        currentWeapon = Weapons[currentIndex];
        currentWeapon.weapon.SetActive(true);

        states = new PlayerStateFactory(this);

        currentState = states.InGroungState();
        currentState.OnStateEnter();

        input.PlayerControl.MoveFowardBackward.performed += OnMoveInput;
        input.PlayerControl.MoveFowardBackward.canceled += OnMoveInput;
        input.PlayerControl.MoveFowardBackward.started += OnMoveInput;
        input.PlayerControl.Jump.performed += OnJumpInput;
        input.PlayerControl.Jump.canceled += OnJumpInput;
        input.PlayerControl.Dash.performed += OnDashInput;
        input.PlayerControl.Sprint.performed += OnSprintInput;
        input.PlayerControl.Sprint.canceled += OnSprintInput;
        input.PlayerControl.Attack.performed += OnAttackInput;
        input.PlayerControl.Attack.canceled += OnAttackInput;
        input.PlayerControl.SwitchtoSword.performed += OnSwitchToSwordInput;
        input.PlayerControl.SwitchtoSword.canceled += OnSwitchToSwordInput;
        input.PlayerControl.SwitchtoDagger.performed += OnSwitchToDaggerInput;
        input.PlayerControl.SwitchtoDagger.canceled += OnSwitchToDaggerInput;
        input.PlayerControl.Defense.performed += OnDefenseInput;
        input.PlayerControl.Defense.canceled += OnDefenseInput;
        input.PlayerControl.Focus.performed += OnFocusInput;
        input.PlayerControl.Focus.canceled += OnFocusInput;

    }

    public void OnDefenseInput(InputAction.CallbackContext context)
    {
        _DefenseButtonPressed = context.ReadValueAsButton();
    }

    public void OnSwitchToDaggerInput(InputAction.CallbackContext context)
    {
        if (context.performed) { 
        currentWeapon.weapon.SetActive(false);
        currentIndex++;
        if (currentIndex >= Weapons.Count)
        {
            currentIndex = 0;
        }
        currentWeapon = Weapons[currentIndex];
        currentWeapon.weapon.SetActive(true);
        }
        //sword = false;
        //animator.SetBool("Sword", false);
        //swordPrefab.SetActive(false);
        //daggerPrefab.SetActive(true);
    }

    public void OnSwitchToSwordInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        currentWeapon.weapon.SetActive(false);
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = Weapons.Count - 1;
        }
        currentWeapon = Weapons[currentIndex];
        currentWeapon.weapon.SetActive(true);
        }
        //sword = true;
        //animator.SetBool("Sword", true);
        //swordPrefab.SetActive(true);
        //daggerPrefab.SetActive(false);
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        _AttackButtonPressed = context.ReadValueAsButton();
    }

    public void OnSprintInput(InputAction.CallbackContext context)
    {
        _SprintButtonPressed = context.ReadValueAsButton();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _DashButtonPressed = true;
        }
        else if (context.canceled)
        {
            _DashButtonPressed = false;
        }
    }

    public void OnFocusInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleFocus();
        }
        
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        _JumpButtonPressed = context.ReadValueAsButton();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx) {
        Vector2 inputVector = ctx.ReadValue<Vector2>();
        
        _WalkButtonPressed = inputVector.x != 0 || inputVector.y != 0;
        moveInput = inputVector;

    }

    public void OnNextFocusInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraScript.NextEnemy();
        }
    }

    public void OnPreviousFocusInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraScript.PreviousEnemy();
        }
    }

    private void Update()
    {
        currentState.OnStateUpdate();
        
        
    }

    public void Die()
    {
        currentState = states.DeadState();
    }

    public void ToggleFocus()
    {
        if (!focused)
        {
            
            cameraScript.StartCombatFocus();
        }
        else
        {
            
            cameraScript.StopCombatFocus();
        }
    }
}
    
