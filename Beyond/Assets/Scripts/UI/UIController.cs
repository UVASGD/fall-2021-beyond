using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * By Ryan Grayson and Eric Weng
 */
public class UIController : MonoBehaviour
{
    /* UI */
    // TODO add start message
    [SerializeField] private GameObject menuButton;
    [SerializeField] private Text titleText;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private Text timesUpText;

    /* Game */
    private Timer timer; // timer for the game

    private void Start()
    {
        timer = GetComponent<Timer>();
        ResetGame();
    }

    private void Update()
    {
        if (timer.IsStarted())
        {
            int minutes = Mathf.FloorToInt(timer.value / 60f);
            int seconds = Mathf.FloorToInt(timer.value - minutes * 60f);
            string timeStr = string.Format("{0:0}:{1:00}", minutes, seconds);

            timerText.text = timeStr;
            timerSlider.value = timer.value;
        }
    }

    public void ResetGame() // set up the level
    {
        // Set UI Visibility
        titleText.enabled = true;
        timesUpText.enabled = false;
        timerSlider.enabled = false;
        timerSlider.maxValue = timer.maxValue;
        timerSlider.value = timerSlider.maxValue;
        menuButton.SetActive(true);

        timerText.text = "Press space to start";
        timerSlider.enabled = false;
    }

    public void StartGame()
    {
        // Set UI visibility
        titleText.enabled = false;
        menuButton.SetActive(false);
        timerSlider.enabled = true;
    }

    public void EndGame()
    {
        // Set UI Visibility
        titleText.enabled = true;
        timesUpText.enabled = true;
        timerText.enabled = false;
        // issue: timer bar disappearing
    }

}
