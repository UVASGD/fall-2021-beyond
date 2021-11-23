using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NextMind;
using NextMind.Calibration;
using NextMind.Devices;

/// <summary>
/// From NextMind quick start guide: https://www.next-mind.com/documentation/unity-sdk/tutorials/03-customizing-calibration/
/// </summary>
public class CalibrationManager : MonoBehaviour
{
    [SerializeField] private Text resultsText;

    private void OnReceivedResults(Device device, CalibrationResults.CalibrationGrade grade)
    {
        resultsText.text = $"Calibration grade for device {device.Name}: {grade}";
        Debug.Log(resultsText.text);
    }
}
