using UnityEngine;

/**
 * By Eric Weng
 */
public class SceneController : MonoBehaviour
{
    public enum SceneState
    {
        LOADING,
        RUNNING,
        GAMEOVER
    }

    private SceneState state;
    private Timer timer; // the timer script

    private void Start()
    {
        state = SceneState.LOADING;
        timer = GetComponent<Timer>();
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
                    timer.startTimer();
                }
                break;
            case SceneState.RUNNING:
                if (timer.isTimerStopped())
                {
                    state = SceneState.GAMEOVER;
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
