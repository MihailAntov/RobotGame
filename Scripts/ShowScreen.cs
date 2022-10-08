using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ShowScreen : MonoBehaviour
{
    CharacterController context;
    void Awake()
    {
        context = GetComponent<CharacterController>();
        CinemachineBrain brain = GetComponent<CinemachineBrain>();
        CinemachineVirtualCamera screenCamera = GameObject.Find("ScreenCam").GetComponent<CinemachineVirtualCamera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
