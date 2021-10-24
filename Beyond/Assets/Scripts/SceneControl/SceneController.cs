using UnityEngine;

/**
 * By Eric Weng
 */
public class SceneController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerScript; 
    private UIController uiTimer;
    private SceneState state;

    private void Start()
    {
        state = SceneState.LOADING;
        uiTimer = GetComponent<UIController>();
        //playerScript.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<ChangeScene>().LoadScene("MenuScene"); // exit to menu
        }

        
        switch (state)
        {
            case SceneState.LOADING:
                if (Input.GetKeyDown(KeyCode.Space)) // Press space to start
                {
                    state = SceneState.RUNNING;
                    //playerScript.enabled = true; // somehow jump breaks with this
                    uiTimer.StartGame();
                }
                break;

            case SceneState.RUNNING:
                if (uiTimer.IsGameOver()) // End Game
                {
                    state = SceneState.GAMEOVER;
                    //playerScript.enabled = false;
                }
                // TODO detect player victory
                break;

            case SceneState.GAMEOVER:
                break;
        }
    }

    public SceneState GetState()
    {
        return state;
    }
}
