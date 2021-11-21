using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A small script to toggle the crosshair visiblity and color.
/// </summary>
public class CrosshairRaycast : MonoBehaviour
{
    [SerializeField] private Sprite crosshairInactive;
    [SerializeField] private Sprite crosshairActive;
    [SerializeField] private Transform raycastSource;
    [SerializeField] private float range = 20;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (image.enabled)
        {
            if (Physics.Raycast(raycastSource.position, raycastSource.forward, out RaycastHit hit))
            {
                image.sprite = (hit.distance <= range) ? crosshairActive : crosshairInactive;
            }
        }
    }

    public void SetVisible(bool enabled)
    {
        image.enabled = enabled;
    }
}
