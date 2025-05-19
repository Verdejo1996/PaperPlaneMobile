using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour
{
    public GameObject staticScene;     // Edificio y ventana
    public GameObject dynamicWorld;    // Contenedor de spawners y scroll
    public GameObject pauseMenuUI;
    public TextMeshProUGUI gameText;
    public TextMeshProUGUI endText;

    private bool gameStarted = false;

    public bool beforeStart;
    public bool endGame;
    private bool isPaused = false;

    public Spawn_Controller[] spawners;
    public ScrollerWorld[] scrollingElements;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        beforeStart = true;
        endGame = false;
        if (beforeStart)
        {
            gameText.text = "Rota el telefono para guiar el avion.";
        }
    }

    private void Update()
    {
        if (endGame)
        {
            endText.text = "Buen intento! Prueba otra vez.";
        }
    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        beforeStart = false;

        staticScene.SetActive(false);
        dynamicWorld.SetActive(true);
        
    }

    public void TouchPause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        if(gameStarted)
        {
            Time.timeScale = 1f;
        }
        isPaused = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
