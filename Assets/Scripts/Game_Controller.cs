using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    public GameObject staticScene;     // Edificio y ventana
    public GameObject dynamicWorld;    // Contenedor de spawners y scroll

    private bool gameStarted = false;

    public Spawn_Controller[] spawners;
    public ScrollerWorld[] scrollingElements;

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        staticScene.SetActive(false);
        dynamicWorld.SetActive(true);
        
    }
}
