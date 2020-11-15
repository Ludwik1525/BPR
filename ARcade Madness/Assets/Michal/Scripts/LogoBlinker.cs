using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoBlinker : MonoBehaviour
{
    public Image topLogo, welcomeTextLight, logoLight1, logoLight2, bottomLight;

    public Text welcomeText, bottomLogo;

    Color32 colorOn = new Color32(255, 5, 0, 255);

    int counter = 255;
    bool isDecreasing = true;
    Color32 colorMain = new Color32(255, 255, 255, 255);
    

    private void OnEnable()
    {
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
            if(counter >= 300)
            {
                isDecreasing = true;
            }
        }

        if (counter <= 255)
        {
            welcomeText.GetComponent<Outline>().effectColor = new Color32(204, 0, (byte)(counter-120), 255);
            welcomeTextLight.color = new Color32(255, 180, (byte)(counter - 100), 120);
            topLogo.color = new Color32((byte)counter, (byte)counter, (byte)counter, 255);
            logoLight1.color = new Color32((byte)counter, (byte)counter, (byte)counter, 120);
            logoLight2.color = new Color32((byte)counter, (byte)counter, (byte)counter, 120);
        }
    }

    IEnumerator BlinkingRoutine()
    {
        while(true)
        {
            byte colOff = (byte)Random.Range(0, 130);
            Color32 colorOff = new Color32(colOff, 5, 0, 255);
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
            colorOn = new Color32(colOn, 5, 0, 255);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }
}
