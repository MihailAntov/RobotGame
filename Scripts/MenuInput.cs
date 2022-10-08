using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class MenuInput : MonoBehaviour
{
    PlayerInput _playerInput;
    [SerializeField]
    Canvas pauseMenu;
    [SerializeField]
    CinemachineVirtualCamera _menuCam;
    CinemachineBrain _brain;
    PlayerStateMachine _context;

    
    void Awake()
    {
        _playerInput = GameObject.Find("Player").GetComponent<PlayerStateMachine>()._playerInput;
        _brain = GameObject.FindObjectOfType<CinemachineBrain>();
        _playerInput.CharacterControls.Menu.started += OnMenuInput;
        _context = FindObjectOfType<PlayerStateMachine>();
        pauseMenu.enabled = false;
        _menuCam.Priority = 1;
    }

    
    public void OnMenuInput(InputAction.CallbackContext context)
    {
        if(pauseMenu.enabled == false && !_context.IsUsingTerminal)
        {
            pauseMenu.enabled = true;
            Cursor.visible = true;
            _playerInput.CharacterControls.Disable();
            _context._normalPov.m_HorizontalAxis.m_MaxSpeed = 0;
            _context._normalTransposer.m_XAxis.m_MaxSpeed = 0;
            _context._lookPov.m_HorizontalAxis.m_MaxSpeed = 0;
            _context._lookTransposer.m_XAxis.m_MaxSpeed = 0;
            _context._crouchPov.m_HorizontalAxis.m_MaxSpeed = 0;
            _context._crouchTransposer.m_XAxis.m_MaxSpeed = 0;
                
            
            
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
