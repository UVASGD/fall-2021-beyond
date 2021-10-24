using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By Eric Weng
 */
public class SceneController : MonoBehaviour
{
    private SceneState state;
    private UIController timer; // the timer script

    private void Start()
    {
        state = SceneState.LOADING;
        timer = GetComponent<UIController>();
    }

    private void Update()
    {
        // Press space to start
        switch (state)
        {
            case SceneState.LOADING:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = SceneState.RUNNING;
                    //timer.startTimer();
                }
                break;
            case SceneState.RUNNING:
                if (timer.isTimerStopped())
                {
                    //state = SceneState.GAMEOVER;
                }
                // TODO detect player victory
                break;
            case SceneState.GAMEOVER:
                break;
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("MenuScene");
        }
    }

    public SceneState GetState()
    {
        return state;
    }
}
