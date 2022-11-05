using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lights : MonoBehaviour
{
    PlayerInput _playerInput;
    AudioManager _audioManager;
    //Light mainLights;
    Light rightEye;
    Light leftEye;
    [SerializeField]
    private GameObject leftEyeLamp;
    [SerializeField]
    private GameObject rightEyeLamp;
    
    
    bool eyesOn = false;
    List<Light> allLights;
    void Awake()
    {
        
        rightEye = GameObject.Find("Right Eye").GetComponent<Light>();
        leftEye = GameObject.Find("Left Eye").GetComponent<Light>();
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Flashlight.started += OnFlashlightInput;
        
        rightEye.intensity = 0;
        leftEye.intensity = 0;
        _audioManager = FindObjectOfType<AudioManager>();
        
        
    }
    void OnFlashlightInput(InputAction.CallbackContext context)
    {
        _audioManager.Play("flashlight");
        if(eyesOn)
        {
            rightEye.intensity = 0;
            leftEye.intensity = 0;
            leftEyeLamp.SetActive(false);
            rightEyeLamp.SetActive(false);

            eyesOn = false;
            
        }
        else
        {
            rightEyeLamp.SetActive(true);
            leftEyeLamp.SetActive(true);
            rightEye.intensity = 3;
            leftEye.intensity = 3;
            eyesOn = true;
            
        } 
        
    }
    
    

    private void OnEnable() {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable() {
        _playerInput.CharacterControls.Disable();
    }
}
