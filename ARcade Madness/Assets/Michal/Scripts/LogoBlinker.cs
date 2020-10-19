using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoBlinker : MonoBehaviour
{
    private Image logo;
    private Image logo2;
    Color32 colorOn = new Color32(255, 255, 255, 255);

    byte counter = 255;
    bool isDecreasing = true;
    Color32 colorMain = new Color32(255, 255, 255, 255);

    void Start()
    {
        logo = transform.GetChild(0).GetComponent<Image>();
        logo2 = GetComponent<Image>();
        StartCoroutine("BlinkingRoutine");
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
        logo.color = colorMain;
    }

    IEnumerator BlinkingRoutine()
    {
        while(true)
        {
            byte colOff = (byte)Random.Range(0, 130);
            Color32 colorOff = new Color32(colOff, colOff, colOff, 255);
            logo2.color = colorOff;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.6f));
            logo2.color = colorOn;
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
}
