using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameHandler : MonoBehaviour {

    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;

    void Awake (){ 
        SetLevel (volumeLevel);
        GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
        if (sliderTemp != null){
            sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
            sliderVolumeCtrl.value = volumeLevel;
        }
    }

    void Start (){
        Resume();
    }

    void Update (){
        if (Input.GetButtonDown("Cancel")){
            Debug.Log("Game is Paused:" + GameisPaused);
            if (GameisPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
        if (SceneManager.GetActiveScene().name == "MainMenu" ||
        SceneManager.GetActiveScene().name == "LoseScene" || 
        SceneManager.GetActiveScene().name == "WinScene") {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Pause(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        if (SceneManager.GetActiveScene().name != "MainMenu" && 
        SceneManager.GetActiveScene().name != "LoseScene" && 
        SceneManager.GetActiveScene().name != "WinScene") {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    public void SetLevel (float sliderValue){
        mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
        volumeLevel = sliderValue;
    }

    public void StartGame() {
        SceneManager.LoadScene("library");
    }

    public void RestartGame() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}