using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject baseMenu;
    [SerializeField]
    GameObject settings;
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
    public float _sensitivity;
    

    private void Awake() {
        settings.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        //_context = FindObjectOfType<PlayerStateMachine>();
        _sensitivity = PlayerPrefs.GetFloat("sensitivity", 1.0f);
        

    }
    private void Start() {
        audioManager.Play("mainMenuMusic");
        //_volumeSlider.value = AudioListener.volume;
        _sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity", 100);
        
        _volumeSlider.value = PlayerPrefs.GetFloat("volume", 1.0f);
        _volumeSliderValue.text = $"{(int)(100 * _volumeSlider.value)}%";
        AudioListener.volume = _volumeSlider.value;
        _graphicsSlider.value = PlayerPrefs.GetInt("graphics", 2);
        switch (_graphicsSlider.value)
        {
            case 1:
                _graphicsSliderValue.text = "Low";
                break;
            case 2:
                _graphicsSliderValue.text = "Medium";
                break;
            case 3:
                _graphicsSliderValue.text = "High";
                break;
        }
        

        ApplyGraphicsSettings((int)_graphicsSlider.value);



    }
    
    public void PlayGame()
    {
        DontDestroyOnLoad(audioManager);
        audioManager.Stop("mainMenuMusic");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        audioManager.Play("Click");
        
    }
    public void ExitGame()
    {
        {
            audioManager.Play("Click");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
        }
    }

    public void Settings()
    {
        baseMenu.SetActive(false);
        settings.SetActive(true);
        //
        _sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity", 100);
        _volumeSlider.value = PlayerPrefs.GetFloat("volume", 1.0f);
        AudioListener.volume = _volumeSlider.value;
        _graphicsSlider.value = PlayerPrefs.GetInt("graphics", 2);
        //
        audioManager.Play("Click");
    }

    public void Back()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume",1.0f);
        _volumeSlider.value = PlayerPrefs.GetFloat("volume",1.0f);
        //_context.Sensitivity = PlayerPrefs.GetInt("sensitivity", 100);
        _sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity", 100);
        //_graphicsSlider.value = PlayerPrefs.GetInt("graphics",2);



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
    public void Apply()
    {
        PlayerPrefs.SetFloat("volume", _volumeSlider.value);
        PlayerPrefs.SetInt("sensitivity", (int)_sensitivitySlider.value);
        PlayerPrefs.SetInt("graphics", (int)_graphicsSlider.value);
        ApplyGraphicsSettings((int)_graphicsSlider.value);

        baseMenu.SetActive(true);
        settings.SetActive(false);
        audioManager.Play("Click");
        
    }

    public void MainVolume(System.Single vol){
        
        AudioListener.volume = vol;
        string value = $"{(int)(100* vol)}%";
        _volumeSliderValue.text = value;
       
  }
  public void MainSensitivity(System.Single sens){
       
        string value = $"{ (int)(sens)}%";
        _sensitivitySliderValue.text = value;

    }

    public void Graphics(System.Single graphics)
    {
        
        string value = string.Empty;
        switch(graphics)
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
}
