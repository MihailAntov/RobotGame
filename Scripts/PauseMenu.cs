using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    PlayerInput _playerInput;
    [SerializeField]
    Canvas pauseMenu;
    PlayerStateMachine _context;
    [SerializeField]
    GameObject settings;
    [SerializeField]
    GameObject baseMenu;
    [SerializeField]
    Slider _volumeSlider;
    [SerializeField]
    Slider _sensitivitySlider;
    [SerializeField]
    Slider _graphicsSlider;
    [SerializeField]
    TextMeshProUGUI _volumeSliderValue;
    [SerializeField]
    TextMeshProUGUI _sensitivitySliderValue;
    [SerializeField]
    TextMeshProUGUI _graphicsSliderValue;


    AudioManager audioManager;

    private void Awake() {
        _playerInput = GameObject.Find("Player").GetComponent<PlayerStateMachine>()._playerInput;
        _context = FindObjectOfType<PlayerStateMachine>();
        audioManager = FindObjectOfType<AudioManager>();
        settings.SetActive(false);
        
    }

    private void Start()
    {
        _volumeSlider.value = AudioListener.volume;
        _sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity", 100);
        _graphicsSlider.value = PlayerPrefs.GetInt("graphics", 2);
    }
    public void QuitGame()
    {
        foreach(Sound s in _context.AudioManager.sounds)
        {
            _context.AudioManager.Stop(s.name);
        }
        SceneManager.LoadScene(0);
        //SceneManager.UnloadSceneAsync(1);
    }

    public void Settings()
    {
        baseMenu.SetActive(false);
        settings.SetActive(true);
        audioManager.Play("Click");
    }

    public void Back()
    {
        //deactivate menu
        baseMenu.SetActive(true);
        settings.SetActive(false);

        //reset volume 
        _volumeSlider.value = PlayerPrefs.GetFloat("volume");
        AudioListener.volume = _volumeSlider.value;
        string volValue = $"{(int)(100 * _volumeSlider.value)}%";
        _volumeSliderValue.text = volValue;

        //reset sens
        _sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity");
        _context.Sensitivity = _sensitivitySlider.value;
        string sensValue = $"{(int)(_sensitivitySlider.value)}%";
        _sensitivitySliderValue.text = sensValue;
        //reset graphics
        _graphicsSlider.value = PlayerPrefs.GetInt("graphics");
        string graphicsValue = string.Empty;
        switch (_graphicsSlider.value)
        {
            case 1:
                graphicsValue = "Low";
                break;
            case 2:
                graphicsValue = "Medium";
                break;
            case 3:
                graphicsValue = "High";
                break;
        }
        _graphicsSliderValue.text = graphicsValue;
        audioManager.Play("Click");

    }

    public void Apply()
    {
        PlayerPrefs.SetInt("sensitivity", (int)_sensitivitySlider.value);
        PlayerPrefs.SetFloat("volume", _volumeSlider.value);
        PlayerPrefs.SetInt("graphics", (int)_graphicsSlider.value);
        ApplyGraphicsSettings((int)_graphicsSlider.value);
        baseMenu.SetActive(true);
        settings.SetActive(false);
        audioManager.Play("Click");
    }
    public void ApplyGraphicsSettings(int value)
    {
        switch (_graphicsSlider.value)
        {
            case 1:
                QualitySettings.SetQualityLevel(0);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
            case 3:
                QualitySettings.SetQualityLevel(4);
                break;
        }
    }

    public void Graphics(System.Single graphics)
    {

        string value = string.Empty;
        switch (graphics)
        {
            case 1:
                value = "Low";
                break;
            case 2:
                value = "Medium";
                break;
            case 3:
                value = "High";
                break;
        }
        _graphicsSliderValue.text = value;

    }

    public void ResumeGame()
    {
        _playerInput.CharacterControls.Enable();
        pauseMenu.enabled = false;
        Cursor.visible = false;

        _context._normalPov.m_HorizontalAxis.m_MaxSpeed = _context.Sensitivity;
        _context._normalTransposer.m_XAxis.m_MaxSpeed = _context.Sensitivity;
        _context._lookPov.m_HorizontalAxis.m_MaxSpeed = _context.Sensitivity;
        //introduce ratio for sensitivity => vertical speed
        _context._lookPov.m_VerticalAxis.m_MaxSpeed = _context.Sensitivity / 3.0f;

        _context._lookTransposer.m_XAxis.m_MaxSpeed = _context.Sensitivity;
        _context._crouchPov.m_HorizontalAxis.m_MaxSpeed = _context.Sensitivity;
        _context._crouchTransposer.m_XAxis.m_MaxSpeed = _context.Sensitivity;
        
        
        
    }

    public void MainVolume(System.Single vol)
    {

        AudioListener.volume = vol;
        //PlayerPrefs.SetFloat("volume",vol);
        string value = $"{(int)(100 * vol)}%";
        _volumeSliderValue.text = value;

    }
    public void MainSensitivity(System.Single sens)
    {

        string value = $"{(int)(sens)}%";
        //PlayerPrefs.SetInt("sensitivity", (int)sens);
        _context.Sensitivity = sens;
        _sensitivitySliderValue.text = value;

    }

}
