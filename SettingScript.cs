using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
    public Button playsettingsButton;
    public Button exitSettingsButton;

    public GameObject playScreen;
    public GameObject settingsScreen;

    private bool isPlayScreenActive;

    // Start is called before the first frame update
    void Start()
    {
        isPlayScreenActive = true;
        playScreen.SetActive(isPlayScreenActive);
        settingsScreen.SetActive(!isPlayScreenActive);

        playsettingsButton.onClick.AddListener(swapCanvas);
        exitSettingsButton.onClick.AddListener(swapCanvas);
    }

    void swapCanvas()
    {
        isPlayScreenActive = !isPlayScreenActive;
        playScreen.SetActive(isPlayScreenActive);
        settingsScreen.SetActive(!isPlayScreenActive);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
