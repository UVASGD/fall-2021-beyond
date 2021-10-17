using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Slider timerSlider;
    public Text timerText;
    public float gameTime;
    private bool stopTimer;
    public Text timesUp;

    public Animator outOfTime;

    public float timer = 0;

    void Start()
    {
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    void Update()
    {
            timer += Time.deltaTime;
            float time = gameTime - timer;

            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);

            string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            if (time <= 0){
                stopTimer = true;
                timesUp.text = "Time's Up!";
                LoadScene("MenuScene");
            }

            if (stopTimer == false){
                timerText.text = textTime;
                timerSlider.value = time;
            }
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(wait(sceneName));
    }
    IEnumerator wait(string sceneName){
        outOfTime.SetTrigger("GameOver");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
