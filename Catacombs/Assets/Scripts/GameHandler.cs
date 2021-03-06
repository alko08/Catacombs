using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityStandardAssets.CrossPlatformInput;

public class GameHandler : MonoBehaviour {

    public static bool GameisPaused = false;
    private GameObject pauseMenuUI, instructMenu;
    public AudioMixer mixer;
    private static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;
    private static string SceneDied = "MainMenu";
    private bool menu;

    void Awake (){ 
        SetLevel (volumeLevel);
        GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
        if (sliderTemp != null){
            sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
            sliderVolumeCtrl.value = volumeLevel;
        }
    }

    void Start (){
        pauseMenuUI = GameObject.FindWithTag("PauseMenu").transform.GetChild(0).gameObject;
        instructMenu = GameObject.FindWithTag("Instructions");
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            GameObject.FindWithTag("keyboard").SetActive(false);
        }

        menu = SceneManager.GetActiveScene().name == "MainMenu" ||
        SceneManager.GetActiveScene().name == "LoseScene" || 
        SceneManager.GetActiveScene().name == "WinScene";
        Resume();
        string thisLevel = SceneManager.GetActiveScene().name;
        if (thisLevel != "LoseScene"){
            SceneDied = thisLevel;
        }
    }

    void Update (){
        if (Input.GetButtonDown("Cancel")){
            // Debug.Log("Game is Paused:" + GameisPaused);
            if (GameisPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
        if (menu) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ReplayGame (){
        SceneManager.LoadScene(SceneDied);
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
        instructMenu.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        if (menu) {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    public void Controls(){
        pauseMenuUI.SetActive(false);
        instructMenu.SetActive(true);
    }

    public void PauseResume(){
        pauseMenuUI.SetActive(true);
        instructMenu.SetActive(false);
    }

    public void SetLevel (float sliderValue){
        mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
        volumeLevel = sliderValue;
        // Debug.Log("Volume: " + (volumeLevel * 20));
    }

    public void StartGame() {
        SceneManager.LoadScene("Levelzeroscene");
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