using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ScreenBehaviour : MonoBehaviour
{

    bool _nearTerminal = false;
    bool _isLookingAtTerminal = false;
    [SerializeField]
    CinemachineVirtualCamera mainCamera;
    [SerializeField]
    CinemachineVirtualCamera screenCamera;
    PlayerInput _playerInput;
    CharacterController characterController;
    PlayerStateMachine Ctx;
    PuzzleBehaviour puzzleBehaviour;
    [SerializeField]
    string puzzleName;
    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Use.started += OnUse;
        _playerInput.PuzzleControls.Exit.started += OnExit;
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();
        
        
        
        
        Ctx = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
        try
        {
            puzzleBehaviour = GameObject.Find(puzzleName).GetComponent<PuzzleBehaviour>();
        }
        catch
        {
            Debug.Log($"No puzzle found! - {puzzleName}");
        }
        
    }
    void OnUse(InputAction.CallbackContext context)
    {
        if (_nearTerminal && !_isLookingAtTerminal)
        {
                _isLookingAtTerminal = true;
                mainCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = 0;
                mainCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
                mainCamera.GetComponent<CinemachineInputProvider>().enabled = false;

                mainCamera.Priority = 0;
                screenCamera.Priority = 9;
                Ctx.IsUsingTerminal = true;
                
                _playerInput.PuzzleControls.Enable();
                _playerInput.CharacterControls.Disable();
                
                Ctx.AppliedMovementX = 0;
                Ctx.AppliedMovementZ = 0;
                Ctx.IsMovementPressed = false;
                puzzleBehaviour.enabled = true;
                puzzleBehaviour.selectedPiece.Object.GetComponent<Outline>().enabled = true;
                puzzleBehaviour.controlsLocked = true;
                puzzleBehaviour.timeCount = puzzleBehaviour.delayNeeded -1;
                //characterController.enabled = false;
                Cursor.visible = true;  
        }
    }
    void OnExit(InputAction.CallbackContext context)
        {
                if(!puzzleBehaviour.controlsLocked)
                {
                _isLookingAtTerminal = false;
                mainCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = 120f;
                mainCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 120f;
                mainCamera.GetComponent<CinemachineInputProvider>().enabled = true;
                mainCamera.m_Transitions.m_InheritPosition = false;
                mainCamera.Priority = 9;
                screenCamera.Priority = 0;
                mainCamera.m_Transitions.m_InheritPosition = true;
                //characterController.enabled = true;
                Ctx.IsUsingTerminal = false;
                _playerInput.PuzzleControls.Disable();
                _playerInput.CharacterControls.Enable();
                puzzleBehaviour.enabled = false;
                puzzleBehaviour.selectedPiece.Object.GetComponent<Outline>().enabled = false;
                Cursor.visible = false;
                }
                
        }


    
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            _nearTerminal = true;
            Debug.Log("Near Terminal");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            _nearTerminal = false;
            Debug.Log("NotNearTerminal");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable() {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable() {
        _playerInput.CharacterControls.Disable();
    }
}
