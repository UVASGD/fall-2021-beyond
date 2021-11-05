using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed;
    private bool fadeIn, fadeOut;

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            Color objectColor = this.GetComponent<Renderer>().material.color;
            float fadeAmount = fadeSpeed * Time.deltaTime;

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, objectColor.a - fadeAmount);

            if (objectColor.a <= 0)
                fadeOut = false;
        }

        if (fadeIn)
        {
            Color objectColor = this.GetComponent<Renderer>().material.color;
            float fadeAmount = fadeSpeed * Time.deltaTime;

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, objectColor.a + fadeAmount);

            if (objectColor.a >= 1)
                fadeIn = false;
        }
    }

    public void FadeInObject()
    {
        fadeIn = true;
    }

    public void FadeOutObject()
    {
        fadeOut = true;
    }
}
