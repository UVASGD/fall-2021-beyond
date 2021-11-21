using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The overall manager script for the entire scene.
/// </summary>
public class SceneController : MonoBehaviour
{
    /* Other Scripts */
    [SerializeField] private PlayerMovement playerScript;
    private UIController uiController;
    private Timer timer; // timer for the game
    public SceneState state { get; private set; }

    /* Level Sections */
    [SerializeField] private List<GameObject> sections = new List<GameObject>(); //The floor for each section
    private int sectionIdx = 0; // What section the player is on

    private void Start()
    {
        state = SceneState.LOADING;
        uiController = GetComponent<UIController>();
        timer = GetComponent<Timer>();
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
                    StartGame();
                }
                break;

            case SceneState.RUNNING:
                if (timer.IsReady()) // End Game
                {
                    EndGame();
                }
                break;

            case SceneState.GAMEOVER:
                break;
        }
    }

    public void StartGame()
    {
        timer.SetStarted(true); // Start counting down
        playerScript.enabled = true;
        uiController.StartGame();
        state = SceneState.RUNNING;
    }

    public void SetNextSection() // Player has passed checkpoint
    {
        sections[sectionIdx++].GetComponent<Rigidbody>().isKinematic = false;
        timer.Reset();
    }

    public void EndGame()
    {
        state = SceneState.GAMEOVER;
        playerScript.enabled = false;
        uiController.EndGame();
        GetComponent<ChangeScene>().LoadScene("MenuScene"); // exit to menu
    }
}
