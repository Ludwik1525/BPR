using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoBlinker : MonoBehaviour
{
    public Image topLogo;
    public Image bottomLogo;

    public Image topLight;
    public Image bottomLight;

    public Text welcomeText;

    Color32 colorOn = new Color32(255, 255, 255, 255);

    byte counter = 255;
    bool isDecreasing = true;
    Color32 colorMain = new Color32(255, 255, 255, 255);
    

    private void OnEnable()
    {
        StartCoroutine("BlinkingRoutine");
        StartCoroutine("TextBlinkerRoutine");
    }

    private void Update()
    {
        if(isDecreasing)
        {
            counter--;
            if(counter < 150)
            {
                isDecreasing = false;
            }
        }
        else
        {
            counter++;
            if(counter >= 255)
            {
                isDecreasing = true;
            }
        }

        colorMain = new Color32(counter, counter, counter, 255);
        topLogo.color = colorMain;
        topLight.color = new Color32(counter, counter, counter, 180);
    }

    IEnumerator BlinkingRoutine()
    {
        while(true)
        {
            byte colOff = (byte)Random.Range(0, 130);
            Color32 colorOff = new Color32(colOff, colOff, colOff, 255);
            bottomLogo.color = colorOff;
            bottomLight.gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            bottomLogo.color = colorOn;
            bottomLight.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        }
    }

    IEnumerator ChangeColour()
    {
        while(true)
        {
            byte colOn = (byte)Random.Range(200, 255);
            colorOn = new Color32(colOn, colOn, colOn, 255);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    IEnumerator TextBlinkerRoutine()
    {
        while(true)
        {
            welcomeText.color = new Color32((byte)Random.Range(110, 255), (byte)Random.Range(110, 255), (byte)Random.Range(110, 255), 255);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
