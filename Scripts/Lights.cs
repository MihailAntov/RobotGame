using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lights : MonoBehaviour
{
    PlayerInput _playerInput;
    AudioManager _audioManager;
    Light mainLights;
    Light rightEye;
    Light leftEye;
    [SerializeField]
    private GameObject leftEyeLamp;
    [SerializeField]
    private GameObject rightEyeLamp;
    
    bool mainLightsOn = true;
    bool eyesOn = false;
    List<Light> allLights;
    void Awake()
    {
        mainLights = GameObject.Find("Directional Light").GetComponent<Light>();
        rightEye = GameObject.Find("Right Eye").GetComponent<Light>();
        leftEye = GameObject.Find("Left Eye").GetComponent<Light>();
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Flashlight.started += OnFlashlightInput;
        _playerInput.CharacterControls.Lamps.started += OnLampsInput;
        rightEye.intensity = 0;
        leftEye.intensity = 0;
        _audioManager = FindObjectOfType<AudioManager>();
        //mainLights.intensity = 1;
        
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
    void OnLampsInput(InputAction.CallbackContext context)
    {
        // if(mainLightsOn)
        // {
        //     mainLights.intensity = 0;
        //     mainLightsOn = false;
        //     RenderSettings.ambientIntensity = 0.2f;
        //     RenderSettings.reflectionIntensity = 0.2f;
        // }
        // else
        // {
        //     mainLights.intensity = 1;
        //     mainLightsOn = true;
        //     RenderSettings.ambientIntensity = 1;
        //     RenderSettings.reflectionIntensity = 1;
        // } 
        
    }
    void Update()
    {
        /*
        if(eyesOn)
        {
            rightEye.intensity = 3;
            leftEye.intensity = 3;
            
        }
        else
        {
            rightEye.intensity = 0;
            leftEye.intensity = 0;
        } 

        if(mainLightsOn)
        {
            mainLights.intensity = 1;
        }
        else
        {
            mainLights.intensity = 0;
        }  
        */
    }

    private void OnEnable() {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable() {
        _playerInput.CharacterControls.Disable();
    }
}
