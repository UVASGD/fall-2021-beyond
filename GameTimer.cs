using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Requires Canvas UI with Slider and Text objects
public class GameTimer : MonoBehaviour
{
    public Slider slider1;
    public Text timetext;

    private float maxTime = 100;
    
    private float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = maxTime;


        timetext.text = "0";
        slider1.maxValue = maxTime;
        slider1.minValue = 0;

    }
    // Update is called once per frame
    void Update()
    {
        timetext.text = "" + Time.time;
        slider1.value = maxTime - Time.time;
    }
}
