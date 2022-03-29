using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameHandler : MonoBehaviour {
    private string sceneName;

    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    private Slider sliderVolumeCtrl;

    void Awake (){
        // pauseMenuUI = GameObject.FindWithTag("PauseMenu");
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
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
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