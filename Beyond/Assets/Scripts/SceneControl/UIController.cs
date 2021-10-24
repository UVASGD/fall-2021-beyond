using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * By Ryan Grayson and Eric Weng
 */
public class UIController : MonoBehaviour
{
    // TODO hide title text and menu button during gameplay
    [SerializeField] private GameObject menuButton;
    [SerializeField] private Text gameText;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private Text timesUpText;
    [SerializeField] private Animator outOfTimeAnimator;

    [SerializeField] private float gameDuration; // how long to run timer
    private float timerValue;

    private bool timerStarted = false; // is timer counting
    private bool timerStopped = false; // timer has reached 0

    private void Start()
    {
        timerSlider.maxValue = gameDuration;
        timerSlider.value = gameDuration;
        timerValue = gameDuration;
    }

    private void Update()
    {
        if (timerStarted)
        {
            menuButton.SetActive(false);
            gameText.text = "";

            timerValue -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timerValue / 60);
            int seconds = Mathf.FloorToInt(timerValue - minutes * 60f);

            string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            if (timerValue <= 0)
            {
                timerStopped = true;
                timesUpText.text = "Time's Up!";
                LoadScene("MenuScene");
            }

            if (!timerStopped)
            {
                timerText.text = textTime;
                timerSlider.value = timerValue;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(wait(sceneName));
    }

    IEnumerator wait(string sceneName){
        outOfTimeAnimator.SetTrigger("GameOver");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    public void startTimer()
    {
        timerStarted = true;
    }

    public bool isTimerStopped()
    {
        return timerStopped;
    }
}
