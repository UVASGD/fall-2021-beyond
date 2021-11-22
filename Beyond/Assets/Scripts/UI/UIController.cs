using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// By Ryan Grayson and Eric Weng
///
/// The manager script for the user interface components.
/// </summary>
public class UIController : MonoBehaviour
{
    /* SceneNavigation */
    [SerializeField] private GameObject menuButton;
    [SerializeField] private Image titleImage;

    /* Game */
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private Text gameOverText;

    /* Power Bar */

    [SerializeField] private GameObject glass;
    [SerializeField] private GameObject powerSliderFill;
    [SerializeField] private Slider powerSlider;
    [SerializeField] private CrosshairRaycast crosshair;
    [SerializeField] private Image powerFill;
    [SerializeField] private Sprite powerBarInactive;
    [SerializeField] private Sprite powerBarActive;
    [SerializeField] private float powerBarDuration = 5f; /// how long for power up bar to fill up

    private Vector3 scaleChange = new Vector3(1.8f, 1f, 1f);
    private Vector3 positionChange = new Vector3(430f, 0f, -430f);

    private float powerValue; //for filling up bar
    private float powerUpLeft; //for the actual power up
    private bool powerBarFilling = false;
    private bool powerBarFull = false;
    private bool powerUpActive = false;

    /* Game */
    private Timer timer; // timer for the game
        
    private void Start()
    {
        timer = GetComponent<Timer>();
        SetUpGame();
    }

    private void Update()
    {
        if (timer.IsStarted())
        {
            // Fill up timer
            int minutes = Mathf.FloorToInt(timer.value / 60f);
            int seconds = Mathf.FloorToInt(timer.value - minutes * 60f);
            string timeStr = string.Format("{0:0}:{1:00}", minutes, seconds);

            timerText.text = timeStr;
            timerSlider.value = timer.value;

            // Fill up power bar
            if (Input.GetKey(KeyCode.LeftShift))
            {
                glass.SetActive(true);
                powerSlider.transform.localScale = scaleChange;
                powerFill.sprite = powerBarActive;
                powerSlider.value -= Time.deltaTime;
                crosshair.SetVisible(true);
            }
            else if (!powerBarFull && !(Input.GetKey(KeyCode.LeftShift)))
            {
                glass.SetActive(false);
                powerFill.sprite = powerBarInactive;
                powerSlider.transform.localScale = new Vector3(1f, 1f, 1f);
                powerSlider.value += .2f * Time.deltaTime;
                crosshair.SetVisible(false);
            }
        }
    }

    public void SetUpGame()
    {
        // Set UI Visibility
        titleImage.enabled = true;
        menuButton.SetActive(true);
        gameOverText.enabled = false;

        // Initialize Timer
        timerSlider.gameObject.SetActive(true);
        timerSlider.maxValue = timer.maxValue;
        timerSlider.value = timerSlider.maxValue;
        timerText.text = "Press space to start";

        // Initialize Power Bar
        powerSlider.gameObject.SetActive(false);
        powerValue = powerBarDuration;
        powerSlider.maxValue = powerBarDuration;
        powerSlider.value = 0;
        crosshair.SetVisible(false);
    }

    public void StartGame()
    {
        // Set UI visibility
        titleImage.enabled = false;
        menuButton.SetActive(false);
        timerSlider.gameObject.SetActive(true);
        powerSlider.gameObject.SetActive(true);
    }

    public void EndGame() // Called when Timer reaches 0
    {
        // Set UI Visibility
        titleImage.enabled = true;
        gameOverText.enabled = true;
        timerText.enabled = false;
        timerSlider.gameObject.SetActive(true);
    }

}
