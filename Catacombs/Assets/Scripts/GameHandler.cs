using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {
      private string sceneName;

      void Start(){
            sceneName = SceneManager.GetActiveScene().name;
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