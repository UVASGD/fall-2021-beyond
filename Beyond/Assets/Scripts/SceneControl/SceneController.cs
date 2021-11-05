using UnityEngine;

/**
 * By Eric Weng
 */
public class SceneController : MonoBehaviour
{
    private static SceneController INSTANCE = null;

    [SerializeField] private PlayerMovement playerScript;
    private UIController uiTimer;
    private SceneState state;

    private void Start()
    {
        state = SceneState.LOADING;
        uiTimer = GetComponent<UIController>();
        playerScript.enabled = false; // breaks player jumping unless slide is activated
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
                    playerScript.enabled = true;
                    uiTimer.StartGame();
                    state = SceneState.RUNNING;
                }
                break;

            case SceneState.RUNNING:
                if (uiTimer.IsGameOver()) // End Game
                {
                    EndGame();
                }
                break;

            case SceneState.GAMEOVER:
                break;
        }
    }

    public static SceneState GetState()
    {
        return INSTANCE.state;
    }

    public static void EndGame()
    {
        INSTANCE.playerScript.enabled = false;
        INSTANCE.state = SceneState.GAMEOVER;
        Debug.Log("Ended Game");
    }
}
