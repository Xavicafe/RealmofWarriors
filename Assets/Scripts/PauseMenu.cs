using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public static bool OptionsActive = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private CharacterControl CharacterControl;


    void Start(){
        Camera camaraPrincipal = Camera.main;
        CharacterControl = camaraPrincipal.GetComponent<CharacterControl>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (GameisPaused) { 
                if(OptionsActive){
                    Pause();
                }
                else{
                    Resume();
                }
            }
            else { Pause(); }
        }
    }

    public void Opciones(){
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        OptionsActive=true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        CharacterControl.pausa=GameisPaused;
    }

    public void Pause()
    {
        OptionsActive=false;
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
        CharacterControl.pausa=GameisPaused;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main menu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
