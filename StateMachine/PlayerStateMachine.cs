using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerStateMachine : MonoBehaviour
{

    //getters and setters

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsJumpLocked { get { return _isJumpLocked; } set { _isJumpLocked = value; } }
    public Animator Animator { get { return _animator; } }
    public int IsJumpingHash { get { return _isJumpingHash; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public bool IsJumping { set { _isJumping = value; } get { return _isJumping; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float InitialJumpVelocity { get { return _initialJumpVelocity; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public float Gravity { get { return _gravity; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } set { _isMovementPressed = value; } }
    public bool IsStrafeRightPressed { get { return _isStrafeRightPressed; } }
    public bool IsStrafeLeftPressed { get { return _isStrafeLeftPressed; } }
    public bool IsWalkBackPressed { get { return _isWalkBackPressed; } }
    public bool IsStrafing { get { return _isStrafing; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public Vector3 AppliedMovement { get { return _appliedMovement; } set { _appliedMovement = value; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public float WalkSpeed { get { return _walkSpeed; } }
    public float RunSpeed { get { return _runSpeed; } }
    public bool IsCrouching { get { return _isCrouching; } set { _isCrouching = value; } }
    public bool IsClimbing { get { return _isClimbing; } set { _isClimbing = value; } }
    public Vector3 LedgeCoordinates { get { return _ledgeCoordinates; } set { _ledgeCoordinates = value; } }
    public bool IsHanging { get { return _isHanging; } set { _isHanging = value; } }
    public bool IsRolling { get { return _isRolling; } set { _isRolling = value; } }
    public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
    public bool LetGo { get { return _letGo; } set { _letGo = value; } }

    public bool IsLanding { get { return _isLanding; } set { _isLanding = value; } }
    public float LedgeRotation { get { return _ledgeRotation; } set { _ledgeRotation = value; } }
    public int AnimationTimer { get { return _animationTimer; } set { _animationTimer = value; } }
    public CinemachineBrain Brain { get { return _brain; } set { _brain = value; } }
    public ICinemachineCamera ActiveCamera { get { return _camera; } set { _camera = value; } }
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public Vector3 LastForward { get { return _lastForward; } set { _lastForward = value; } }
    public bool IsDropping { get { return _isDropping; } set { _isDropping = value; } }
    public bool RequireNewDropPress { get { return _requireNewDropPress; } set { _requireNewDropPress = value; } }
    public bool IsCurrentlyDropping { get { return _isCurrentlyDropping; } set { _isCurrentlyDropping = value; } }
    public Vector3 FacingLedge { get { return _facingLedge; } set { _facingLedge = value; } }
    public int JumpLockTimer { get { return _jumpLockTimer; } set { _jumpLockTimer = value; } }
    public bool IsUsingTerminal { get { return _isUsingTerminal; } set { _isUsingTerminal = value; } }
    public bool InLookState { get { return _inLookState; } set { _inLookState = value; } }
    public bool IsLooking { get { return _isLooking; } set { _isLooking = value; } }
    public PlayerStateFactory States { get { return _states; } }
    public AudioManager AudioManager { get { return audioManager; } }
    public string Surface { get { return _surface; } }
    public string[] Surfaces {get {return _surfaces;}}
    public string Footsteps {get {return _footsteps;} set {_footsteps = value;}}
    public string Runsteps {get {return _runsteps;} set {_runsteps = value;}}
    public bool Attached {get {return _attached;} set {_attached = value;}}
    public Transform PreviousParent {get {return _previousParent;} set {_previousParent = value;}}
    public float Sensitivity {get {return _sensitivity;} set {_sensitivity = value;}}
    public GameObject HeadRotationUpDown { get { return _head; } set {_head = value;}}
    
    


    //variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _appliedMovement;
    Vector3 _ledgeCoordinates;
    Vector3 _facingLedge;
    Transform _previousParent;
    float _ledgeRotation;
    float _sensitivity;
    string _surface;
    string [] _surfaces = new string[] {"concrete","metal"};
    string _footsteps;
    string _runsteps;

    bool _isHanging;
    bool _isMovementPressed;
    bool _isStrafeRightPressed;
    bool _isStrafeLeftPressed;
    bool _isWalkBackPressed;
    bool _isStrafing;
    bool _isRunPressed;
    bool _isCrouching;
    bool _isClimbing;
    bool _isFalling;
    bool _isRolling;
    bool _isLanding;
    bool _isDropping;
    bool _isLooking;
    bool _inLookState;
    bool _isCurrentlyDropping;
    bool _isUsingTerminal = false;
    float _walkSpeed = 4f;
    float _runSpeed = 9f;
    int _animationTimer;
    //float _runMultiplier = 1.5f;

    //jump 
    bool _isJumpPressed = false;
    bool _isJumpLocked = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = 2f;
    float _maxJumpTime = 0.75f;
    int _jumpLockTimer = 0;
    bool _isJumping = false;
    float _jumpModifier = 1.15f;
    float _gravity = -5.8f;

    float _currentCameraYRotation;
    int layerMask;
    bool _attached;
    CharacterController _characterController;
    public PlayerInput _playerInput;
    Animator _animator;
    
    AudioManager audioManager;

    float _rotationFactorPerFrame = 6f;
    int _isJumpingHash;
    bool _requireNewJumpPress;
    bool _requireNewDropPress;
    
    bool _canGrabLedge = false;
    bool _letGo = false;
    bool _canMove = true;
    GameObject _head;
    Transform _bodyTransform;
    CinemachineBrain _brain;
    ICinemachineCamera _camera;
    [SerializeField]
    public GameObject _crosshair;
    Vector3 _lastForward;
    [SerializeField]
    public CinemachineVirtualCamera _normalCamera;
    [SerializeField]
    public CinemachineVirtualCamera _crouchCamera;
    [SerializeField]
    public CinemachineVirtualCamera _lookCamera;

    [HideInInspector]
    public CinemachineOrbitalTransposer _normalTransposer;
    [HideInInspector]

    public CinemachinePOV _normalPov;
    [HideInInspector]
    public CinemachineOrbitalTransposer _crouchTransposer;
    [HideInInspector]

    public CinemachinePOV _crouchPov;
    [HideInInspector]
    public CinemachineOrbitalTransposer _lookTransposer;
    [HideInInspector]

    public CinemachinePOV _lookPov;
    [SerializeField]
    public AudioSource footstep;


    private void Awake()
    {

        //initial variables
        _playerInput = new PlayerInput();
        _animator = GetComponent<Animator>();
        
        _characterController = GetComponent<CharacterController>();
        _normalTransposer = _normalCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _normalPov = _normalCamera.GetCinemachineComponent<CinemachinePOV>();
        _crouchTransposer = _crouchCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _crouchPov = _crouchCamera.GetCinemachineComponent<CinemachinePOV>();
        _lookTransposer = _lookCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _lookPov = _lookCamera.GetCinemachineComponent<CinemachinePOV>();
        SetUpJumpVariables();
        Cursor.visible = false;
        _crosshair.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        layerMask = LayerMask.GetMask("moving");
        _previousParent = CharacterController.transform.parent;
        //states
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        //callbacks for input
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += onRun;
        _playerInput.CharacterControls.Run.canceled += onRun;
        _playerInput.CharacterControls.Jump.started += onJump;
        _playerInput.CharacterControls.Jump.canceled += onJump;
        _playerInput.CharacterControls.Crouch.started += onCrouch;
        _playerInput.CharacterControls.Drop.started += onDrop;
        _playerInput.CharacterControls.Look.started += onLook;
        _playerInput.CharacterControls.Look.canceled += onLook;
        _playerInput.CharacterControls.SceneSwitch.started += OnSceneSwitch;
        //_playerInput.CharacterControls.Look.canceled += onLookRelease;
        //_playerInput.CharacterControls.Drop.canceled += onDrop;
        _head = GameObject.Find("HeadOfRobot");
        _bodyTransform = GameObject.Find("Player").transform;

        //animation hash
        _isJumpingHash = Animator.StringToHash("isJumping");

        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);
        if (brain)
        {
            _brain = brain;
        }
        _camera = _brain.ActiveVirtualCamera;
        _sensitivity = PlayerPrefs.GetInt("sensitivity", 100);
        _normalPov.m_HorizontalAxis.m_MaxSpeed = Sensitivity;
        _normalTransposer.m_XAxis.m_MaxSpeed = Sensitivity;
        _lookPov.m_HorizontalAxis.m_MaxSpeed = Sensitivity;

        _lookPov.m_VerticalAxis.m_MaxSpeed= Sensitivity/3.0f;
        _normalPov.m_VerticalAxis.m_MaxSpeed = Sensitivity/3.0f;
        _crouchPov.m_VerticalAxis.m_MaxSpeed = Sensitivity/3.0f;

        _lookTransposer.m_XAxis.m_MaxSpeed = Sensitivity;
        _crouchPov.m_HorizontalAxis.m_MaxSpeed = Sensitivity;
        _crouchTransposer.m_XAxis.m_MaxSpeed = Sensitivity;

        _sensitivity = PlayerPrefs.GetFloat("sensitivity", 1.0f);
        audioManager.Play("wakeUp");

    }
    
    public void HandleStrafeAnimations()
    {



        //-----------------------------------------

        //HandleLookingEnter();
        if (_isMovementPressed)
        {

            if (CurrentMovementInput.x < 0)
            {
                Animator.SetBool("isStrafing", true);
                if (CurrentMovementInput.y < 0)
                {
                    Animator.SetBool("isWalkingBack", false);
                    Animator.SetBool("isStrafingLeft", false);
                    Animator.SetBool("isStrafingRight", false);
                    Animator.SetBool("isStrafingLeftDiag", true);
                    Animator.SetBool("isStrafingRightDiag", false);
                    Animator.SetBool("isStrafingLeftDiagF", false);
                    Animator.SetBool("isStrafingRightDiagF", false);
                }
                else if (CurrentMovementInput.y == 0)
                {
                    Animator.SetBool("isWalkingBack", false);
                    Animator.SetBool("isStrafingLeft", true);
                    Animator.SetBool("isStrafingRight", false);
                    Animator.SetBool("isStrafingLeftDiag", false);
                    Animator.SetBool("isStrafingRightDiag", false);
                    Animator.SetBool("isStrafingLeftDiagF", false);
                    Animator.SetBool("isStrafingRightDiagF", false);
                }
                else
                {
                    Animator.SetBool("isWalkingBack", false);
                    Animator.SetBool("isStrafingLeft", false);
                    Animator.SetBool("isStrafingRight", false);
                    Animator.SetBool("isStrafingLeftDiag", false);
                    Animator.SetBool("isStrafingRightDiag", false);
                    Animator.SetBool("isStrafingLeftDiagF", true);
                    Animator.SetBool("isStrafingRightDiagF", false);
                }

            }
            else if (CurrentMovementInput.x > 0)
            {
                Animator.SetBool("isStrafing", true);
                if (CurrentMovementInput.y < 0)
                {
                    Animator.SetBool("isWalkingBack", false);
                    Animator.SetBool("isStrafingLeft", false);
                    Animator.SetBool("isStrafingRight", false);
                    Animator.SetBool("isStrafingLeftDiag", false);
                    Animator.SetBool("isStrafingRightDiag", true);
                    Animator.SetBool("isStrafingLeftDiagF", false);
                    Animator.SetBool("isStrafingRightDiagF", false);
                }
                else if (CurrentMovementInput.y == 0)
                {
                    Animator.SetBool("isWalkingBack", false);
                    Animator.SetBool("isStrafingLeft", false);
                    Animator.SetBool("isStrafingRight", true);
                    Animator.SetBool("isStrafingLeftDiag", false);
                    Animator.SetBool("isStrafingRightDiag", false);
                    Animator.SetBool("isStrafingLeftDiagF", false);
                    Animator.SetBool("isStrafingRightDiagF", false);
                }
                else
                {
                    Animator.SetBool("isWalkingBack", false);
                    Animator.SetBool("isStrafingLeft", false);
                    Animator.SetBool("isStrafingRight", false);
                    Animator.SetBool("isStrafingLeftDiag", false);
                    Animator.SetBool("isStrafingRightDiag", false);
                    Animator.SetBool("isStrafingLeftDiagF", false);
                    Animator.SetBool("isStrafingRightDiagF", true);
                }
            }
            else if (CurrentMovementInput.y < 0)
            {
                Animator.SetBool("isWalkingBack", true);
                Animator.SetBool("isStrafing", true);
                Animator.SetBool("isStrafingLeft", false);
                Animator.SetBool("isStrafingRight", false);
                Animator.SetBool("isStrafingLeftDiag", false);
                Animator.SetBool("isStrafingRightDiag", false);
                Animator.SetBool("isStrafingLeftDiagF", false);
                Animator.SetBool("isStrafingRightDiagF", false);
            }
            else
            {
                Animator.SetBool("isWalkingBack", false);
                Animator.SetBool("isStrafing", false);
                Animator.SetBool("isStrafingLeft", false);
                Animator.SetBool("isStrafingRight", false);
                Animator.SetBool("isStrafingLeftDiag", false);
                Animator.SetBool("isStrafingRightDiag", false);
                Animator.SetBool("isStrafingLeftDiagF", false);
                Animator.SetBool("isStrafingRightDiagF", false);
            }
        }
        else
        {
            Animator.SetBool("isWalkingBack", false);
            Animator.SetBool("isStrafing", false);
            Animator.SetBool("isStrafingLeft", false);
            Animator.SetBool("isStrafingRight", false);
            Animator.SetBool("isStrafingLeftDiag", false);
            Animator.SetBool("isStrafingRightDiag", false);
            Animator.SetBool("isStrafingLeftDiagF", false);
            Animator.SetBool("isStrafingRightDiagF", false);
        }
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        if (!_isUsingTerminal)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
            if (_isLooking && CurrentState == _states.Grounded())
            {

                HandleStrafeAnimations();
            }
        }
        else
        {
            _isMovementPressed = false;
        }




    }

    public bool IsPlaying(string animationName)
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanDrop()
    {
        if (Physics.Raycast(CharacterController.transform.position + CharacterController.transform.forward * 1f, Vector3.down, out var verticalHit))
        {
            if (verticalHit.distance > 2.0f)
            {
                if (Physics.Raycast(CharacterController.transform.position + CharacterController.transform.forward * 1f + CharacterController.transform.up * -0.1f, CharacterController.transform.forward * -1f, out var horizontalHit))
                //if (Physics.Raycast(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.01f, Ctx.CharacterController.transform.position.z) + Ctx.CharacterController.transform.forward * -1f, Ctx.CharacterController.transform.forward, out var horizontalHit))
                {
                    if (horizontalHit.distance < 1f)
                    {
                        Vector3 edgeRotation = horizontalHit.normal;
                        Vector3 edgePosition = new Vector3(horizontalHit.point.x, CharacterController.transform.position.y - 0.1f, horizontalHit.point.z);
                        float rotationNeeded = Vector3.SignedAngle(CharacterController.transform.forward, edgeRotation, Vector3.up);
                        Debug.Log(rotationNeeded);
                        LedgeCoordinates = edgePosition;
                        LedgeRotation = rotationNeeded;
                        return true;

                    }

                }
            }

        }

        return false;
    }

    public void DetectSurface()
    {
        if(CurrentState == States.Hang() || CurrentState == States.Climb() || CurrentState == States.Jump() || CurrentState == States.Fall())
        {
            _surface = "none";
            _runsteps = string.Empty;
            _footsteps = string.Empty;
            return;
        }
        
        // public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask);
        Ray surfaceRay = new Ray(_bodyTransform.position + Vector3.up*1f, Vector3.down);
        if (Physics.Raycast(surfaceRay, out var hitMoving, 1.2f, layerMask))
        {   
                // if(!_attached)
                // {
                //     _previousParent = CharacterController.transform.parent;
                // }
                _attached = true;
                CharacterController.transform.parent = hitMoving.transform.parent;
        }
        else
        {
            CharacterController.transform.parent = _previousParent;
            _attached = false;
        }

        
       
        if (Physics.Raycast(surfaceRay, out var hitFloor, 1.2f))
        {
            
            if (hitFloor.collider.tag == "metal")
            {
               _surface = "metal";

            }
            else 
            {
                _surface = "concrete";
            }
            

        }
        
        


    }

    void onJump(InputAction.CallbackContext context)
    {
        if (!_isJumpLocked && !_isUsingTerminal)
        {
            _isJumpPressed = context.ReadValueAsButton();
            _requireNewJumpPress = false;
        }


    }





    void onRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }
    void onCrouch(InputAction.CallbackContext context)
    {

        if (!_isUsingTerminal)
        {
            _isCrouching = !_isCrouching;
        }



    }
    void onDrop(InputAction.CallbackContext context)
    {
        if (!_isHanging && CanDrop())
        {
            _isDropping = context.ReadValueAsButton();
            _requireNewDropPress = false;

        }
        else
        {
            _letGo = true;
            _requireNewDropPress = false;

        }
    }

    void onLook(InputAction.CallbackContext context)
    {
        _isLooking = context.ReadValueAsButton();

        // if(_currentState == _states.Grounded())
        // {
        //     _isLooking = true;
        // }


        if (_isLooking && CurrentState == _states.Grounded())
        {

            if (!_inLookState)
            {
                _inLookState = true;
                _crosshair.SetActive(true);
                _lookCamera.Priority = 9;
                _normalCamera.Priority = 1;
                _lookPov.m_HorizontalAxis.Value = _normalPov.m_HorizontalAxis.Value;
                _lookTransposer.m_XAxis.Value = _normalTransposer.m_XAxis.Value;
                HandleStrafeAnimations();
            }

            //-----------------------------------------


        }
        else
        {
            _isLooking = false;

            Animator.SetBool("isWalkingBack", false);
            Animator.SetBool("isStrafing", false);
            Animator.SetBool("isStrafingLeft", false);
            Animator.SetBool("isStrafingRight", false);
            Animator.SetBool("isStrafingLeftDiag", false);
            Animator.SetBool("isStrafingRightDiag", false);
            if (_inLookState)
            {
                _inLookState = false;
                _crosshair.SetActive(false);
                _lookCamera.Priority = 1;
                _normalCamera.Priority = 9;
                _normalPov.m_HorizontalAxis.Value = _lookPov.m_HorizontalAxis.Value;
                _normalTransposer.m_XAxis.Value = _lookTransposer.m_XAxis.Value;
            }
        }




    }
    void onLookRelease(InputAction.CallbackContext context)
    {
        // _isLooking = false;

        // Animator.SetBool("isWalkingBack", false);
        // Animator.SetBool("isStrafing", false);
        // Animator.SetBool("isStrafingLeft", false);
        // Animator.SetBool("isStrafingRight", false);
        // Animator.SetBool("isStrafingLeftDiag", false);
        // Animator.SetBool("isStrafingRightDiag", false);
        // if(_inLookState)
        // {
        // _inLookState = false;
        // _crosshair.SetActive(false);
        // _lookCamera.Priority = 1;
        // _normalCamera.Priority = 9;
        // _normalPov.m_HorizontalAxis.Value = _lookPov.m_HorizontalAxis.Value;
        // _normalTransposer.m_XAxis.Value = _lookTransposer.m_XAxis.Value;
        // }

    }
    
    private void OnSceneSwitch(InputAction.CallbackContext context)
    {
        //SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(2);
            
        
        
        
    }

    void SetUpJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2f * _maxJumpHeight) / timeToApex;
    }

    private void OnEnable()
    {
        //_playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        //_playerInput.CharacterControls.Disable();
    }


    void HandleRotation()
    {
        if (!_inLookState)// || IsCrouching)
        {
            Vector3 positionToLookAt;
            positionToLookAt.x = _appliedMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = _appliedMovement.z;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetHeadRotation = Quaternion.Euler(new Vector3(357, _bodyTransform.rotation.eulerAngles.y - 90, 249));

            _head.transform.rotation = Quaternion.Slerp(_head.transform.rotation, targetHeadRotation, (_rotationFactorPerFrame* 3.0f) * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.fixedDeltaTime);
            if (_isMovementPressed && positionToLookAt != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, (_rotationFactorPerFrame * 3.0f) * Time.fixedDeltaTime);
            }
        }
        
        else
        {
            Quaternion currentRotation = transform.rotation;
            //Quaternion targetRotation = Quaternion.Euler(Camera.main.transform.rotation.x, 0, Camera.main.transform.rotation.z);
            Quaternion targetRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * 3.0f * Time.fixedDeltaTime);
            Quaternion defaultHeadRotation = Quaternion.Euler(new Vector3(357, _bodyTransform.rotation.eulerAngles.y - 90, 249 - (int)(1.4 * (_lookPov.m_VerticalAxis.Value))));

            _head.transform.rotation = Quaternion.Slerp(_head.transform.rotation, defaultHeadRotation, _rotationFactorPerFrame*3.0f * Time.fixedDeltaTime);
            //45 needs to be the vertical axis
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
        audioManager.Play("ambDark");
        StartCoroutine(Fade());

    }
    IEnumerator Fade()
    {
        _playerInput.CharacterControls.Disable();
        yield return new WaitForSeconds(3);
        _playerInput.CharacterControls.Enable();
    }

    // Update is called once per frame
    void Update()
    {

        // HandleRotation();
        // if(CharacterController.enabled == true)
        // {
        //     _characterController.Move(_appliedMovement * Time.deltaTime);
        // }
        // _currentState.UpdateStates();
        // DetectSurface();



    }
    private void FixedUpdate()
    {
        
        
        HandleRotation();
        if(CharacterController.enabled == true)
        {
            _characterController.Move(_appliedMovement * Time.deltaTime);
        }
        _currentState.UpdateStates();
        DetectSurface();
        _currentState.FixedUpdateStates();
    }
    private void OnGUI()
    {


        //GUI.Box(new Rect(20, 50, 200, 25), CurrentState.ToString());
        //GUI.Box(new Rect(20, 100, 200, 25), CurrentState.CurrentSubState.ToString());
        GUI.Box(new Rect(20, 150, 200, 25), "surface  :" + _surface);
        GUI.Box(new Rect(20, 200, 200, 25), "footsteps :" + _footsteps);
        //GUI.Box(new Rect(20, 250, 200, 25), "attached :" + _attached);
        //GUI.Box(new Rect(20, 200, 200, 25), "groundCheck :" + (CurrentState == _states.Grounded()));
        //GUI.Box(new Rect(20, 250, 200, 25), "isStrafing :" + Animator.GetBool("isStrafing"));
        // //GUI.Box(new Rect(20, 300, 200, 25),"":"+ IsHanging);

        //----------------CURRENT ANIMATION--------------- 
        // int w = Animator.GetCurrentAnimatorClipInfo(0).Length;
        // string[] clipName = new string[w];
        // for (int i = 0; i < w; i += 1)
        // {
        //   clipName[i] = Animator.GetCurrentAnimatorClipInfo(0)[i].clip.name;
        // }
        //       GUI.Box(new Rect(20, 250, 200, 25), "anim: " + clipName[0]);

        //------------------------------------------------







    }

}
