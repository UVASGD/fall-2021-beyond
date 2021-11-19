using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * By Eric Weng
 */
public class SceneController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerScript;

    private UIController uiController;
    public SceneState state { get; private set; }

    private void Start()
    {
        state = SceneState.LOADING;
        uiController = GetComponent<UIController>();
        playerScript.enabled = false; // breaks player jumping unless slide is activated
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = SceneState.GAMEOVER;
        }
        
        switch (state)
        {
            case SceneState.LOADING:
                if (Input.GetKeyDown(KeyCode.Space)) // Press space to start
                {
                    playerScript.enabled = true;
                    uiController.StartGame();
                    state = SceneState.RUNNING;
                }
                break;

            case SceneState.RUNNING:
                if (uiController.IsGameOver()) // End Game
                {
                    EndGame();
                }
                break;

            case SceneState.GAMEOVER:
                break;
        }
    }

    public void EndGame()
    {
        playerScript.enabled = false;
        state = SceneState.GAMEOVER;
        Debug.Log("Ended Game");
        GetComponent<ChangeScene>().LoadScene("MenuScene"); // exit to menu
    }
}
