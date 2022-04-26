using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameHandler : MonoBehaviour {

    public static bool GameisPaused = false, showInstruct;
    private GameObject pauseMenuUI, instruct;
    public AudioMixer mixer;
    private static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;
    private static string SceneDied = "MainMenu";

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
        Resume();
        string thisLevel = SceneManager.GetActiveScene().name;
        if (thisLevel != "LoseScene"){
            SceneDied = thisLevel;
        }

        showInstruct = false;
        if (thisLevel != "LoseScene" && thisLevel != "WinScene" && thisLevel != "MainMenu"){
            instruct = GameObject.FindWithTag("Instructions").transform.GetChild(0).gameObject;
            instruct.SetActive(showInstruct);
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
        if (SceneManager.GetActiveScene().name == "MainMenu" ||
        SceneManager.GetActiveScene().name == "LoseScene" || 
        SceneManager.GetActiveScene().name == "WinScene") {
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
        SceneManager.LoadScene("Level_1");
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

    public void ChangeInstruct() {
        showInstruct = !showInstruct;
        instruct.SetActive(showInstruct);
    }
}