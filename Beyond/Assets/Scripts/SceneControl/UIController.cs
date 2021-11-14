using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * By Ryan Grayson and Eric Weng
 */
public class UIController : MonoBehaviour
{
    // TODO hide title text and menu button during gameplay
    [SerializeField] private GameObject menuButton;
    //[SerializeField] private Text startMessage; // TODO not implemented
    [SerializeField] private Text titleText;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private Text timesUpText;

    [SerializeField] private float timerDuration; // how long to run timer
    private float timerValue;

    private bool timerStarted = false; // is timer counting
    private bool timerStopped = false; // timer has reached 0

    private void Start()
    {
        // Set UI visibility
        //startMessage.enabled = false;
        timesUpText.enabled = false;
        titleText.enabled = true;

        // Initialize timer
        timerSlider.enabled = false;
        timerSlider.maxValue = timerDuration;
        timerSlider.value = timerDuration;
        timerValue = timerDuration;
    }

    private void Update()
    {
        if (timerStarted)
        {
            timerValue -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timerValue / 60);
            int seconds = Mathf.FloorToInt(timerValue - minutes * 60f);

            string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            timerText.text = textTime;
            timerSlider.value = timerValue;

            if (timerValue <= 0)
            {
                timerStopped = true;
                timesUpText.enabled = true;
                timesUpText.enabled = true;
                GetComponent<ChangeScene>().LoadScene("MenuScene");
            }
        }
    }

    public void StartGame()
    {
        // Start counting down
        timerStarted = true;

        // Set UI visibility
        titleText.enabled = false;
        timerSlider.enabled = true;
        menuButton.SetActive(false);
    }

    public bool IsGameOver()
    {
        return timerStopped;
    }
}
