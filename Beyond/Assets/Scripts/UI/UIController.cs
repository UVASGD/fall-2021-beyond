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
    // TODO hide title text and menu button during gameplay
    [SerializeField] private GameObject menuButton;
    // TODO add start message
    [SerializeField] private Text titleText;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private Text timesUpText;

    /* Game */
    // TODO move to scene controller
    [SerializeField] private float gameDuration; // how long to run timer
    private Timer timer; // timer for the game
    [SerializeField] private List<GameObject> sections = new List<GameObject>(); //The floor for each section

    private int sectionIdx = 0; // What section the player is on

    private void Start()
    {
        // Set UI visibility
        timesUpText.enabled = false;
        titleText.enabled = true;

        // Initialize timer
        timer = new Timer(gameDuration);
        timerSlider.enabled = false;
        timerSlider.maxValue = timer.maxValue;
        timerSlider.value = timer.value;
    }

    private void Update()
    {
        if (timer.started)
        {
            timer.CountDown(Time.deltaTime);

            int minutes = Mathf.FloorToInt(timer.value / 60f);
            int seconds = Mathf.FloorToInt(timer.value - minutes * 60f);
            string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            timerText.text = textTime;
            timerSlider.value = timer.value;

            if (timer.IsReady())
            {
                timer.started = false;
                timesUpText.enabled = true;
                timesUpText.enabled = true;
                //GetComponent<ChangeScene>().LoadScene("MenuScene");
            }
        }
    }

    public void SetNextSection()
    {
        timesUpText.text = "";
        timer.Reset();
        sections[sectionIdx].GetComponent<Rigidbody>().isKinematic = false;
        sectionIdx++;
        Debug.Log("On section " + sectionIdx);
    }

    public void StartGame()
    {
        // Start counting down
        timer.started = true;

        // Set UI visibility
        titleText.enabled = false;
        timerSlider.enabled = true;
        menuButton.SetActive(false);
    }

    public bool IsGameOver()
    {
        return timer.IsReady();
    }
}
