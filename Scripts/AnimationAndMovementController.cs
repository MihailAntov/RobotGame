using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AnimationAndMovementController : MonoBehaviour
{
    
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isRunPressed;
    float _walkSpeed = 4f;
    float _runSpeed = 7f;

    //jump 
    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = 1.5f;
    float _maxJumpTime = 1.0f;
    bool _isJumping = false;
    float _jumpModifier = 1.0f;
    float _groundedGravity = -0.05f;
    float _gravity = -9.8f;
    
    float _currentCameraYRotation;
    CharacterController _characterController;  
    PlayerInput _playerInput;
    Animator _animator;

    float _rotationFactorPerFrame = 1f;
    int _isJumpingHash;
    bool _isJumpAnimating;
    private void Awake() 
    {
        _isJumpingHash = Animator.StringToHash("isJumping");
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += onRun;
        _playerInput.CharacterControls.Run.canceled += onRun;
        _playerInput.CharacterControls.Jump.started += onJump;
        _playerInput.CharacterControls.Jump.canceled += onJump;
        SetUpJumpVariables();
        _animator = GetComponent<Animator>();
    }
    
    void OnMovementInput(InputAction.CallbackContext context)
    {
        

            _currentMovementInput = context.ReadValue<Vector2>();
            
            // was here
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0 ;
            
    }

    void onJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();

    }


 


    void onRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    void SetUpJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2* _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2f * _maxJumpHeight) / timeToApex;
    }
 
    void HandleJump()
    {
        


        if(!_isJumping && _characterController.isGrounded && _isJumpPressed)
        {
            _animator.SetBool(_isJumpingHash, true);
            _isJumpAnimating = true;
            _isJumping = true;
            _currentMovement.y = _initialJumpVelocity;
            _appliedMovement.y = _initialJumpVelocity;
            
        }
        else if (_isJumping && _characterController.isGrounded && !_isJumpPressed)
        {
            _isJumping = false;
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovement.x ;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _currentMovement.z ;
        Quaternion currentRotation = transform.rotation;
        
        
        if(_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.fixedDeltaTime);
        }
        
    }
    void HandleAnimation()
    {
        bool isWalking = _animator.GetBool("isWalking");
        bool isRunning = _animator.GetBool("isRunning");
        bool hasJumped = _animator.GetBool("hasJumped");

        if (_isMovementPressed && !isWalking)
        {
            _animator.SetBool("isWalking", true);
        }

        if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool("isWalking", false);
        }

        if(isWalking && _isRunPressed)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }

        if(_isJumping)
        {
            _animator.SetBool("hasJumped", true);
        }

        if(!_isJumping)
        {
            _animator.SetBool("hasJumped", false);
        }
    }

    void HandleGravity()
    {
        bool isFalling = _currentMovement.y <= 0.0f || !_isJumpPressed;
        float fallMultiplier = 2.0f;
        if(_characterController.isGrounded)
        {
            if(_isJumpAnimating)
            {
                _animator.SetBool(_isJumpingHash, false);
                _isJumpAnimating = false;
            }
            _currentMovement.y = _groundedGravity;
            _appliedMovement.y = _groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_gravity * fallMultiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((previousYVelocity + _currentMovement.y) * 0.5f, -20f);
            
        }
        else
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_gravity * Time.deltaTime);
            _appliedMovement.y = (previousYVelocity + _currentMovement.y) * 0.5f;
            
        }
    }
    void HandleMovement()
    {
            if(_isJumping)
            {
                _jumpModifier = 1.15f;
            }
            else
            {
                _jumpModifier = 1.0f;
            }
            _currentCameraYRotation = Camera.main.transform.eulerAngles.y;
            _currentMovement.x = _currentMovementInput.x  * _walkSpeed * _jumpModifier;
            _currentMovement.z = _currentMovementInput.y * _walkSpeed * _jumpModifier;
            _currentRunMovement.x = _currentMovementInput.x * _runSpeed * _jumpModifier;
            _currentRunMovement.z = _currentMovementInput.y * _runSpeed * _jumpModifier;            
            _currentMovement = Quaternion.Euler(0f, _currentCameraYRotation, 0f) * _currentMovement;
            _currentRunMovement =   Quaternion.Euler(0f, _currentCameraYRotation, 0f) * _currentRunMovement;

            
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleAnimation();
        
        
        if(_isRunPressed)
        {
            _appliedMovement.x = _currentRunMovement.x;
            _appliedMovement.z = _currentRunMovement.z;
        }
        else
        {
            _appliedMovement.x = _currentMovement.x;
            _appliedMovement.z = _currentMovement.z;
        }

        _characterController.Move(_appliedMovement * Time.deltaTime);
        
        HandleGravity();
        HandleJump();
    }
     private void LateUpdate() 
     {
     }
    private void OnEnable() {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable() {
        _playerInput.CharacterControls.Disable();
    }
}
