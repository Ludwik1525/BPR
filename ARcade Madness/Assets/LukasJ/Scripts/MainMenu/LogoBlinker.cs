using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoBlinker : MonoBehaviour
{
    bool isDecreasing = true;

    int counter = 255;

    public Image topLogo, welcomeTextLight, logoLight1, logoLight2, bottomLight;

    public Text welcomeText, bottomLogo;

    Color32 colorOn = new Color32(255, 5, 0, 255), colorMain = new Color32(255, 255, 255, 255);
    

    private void OnEnable()
    {
        StartCoroutine("BlinkingRoutine");
    }

    private void Update()
    {
        // if the colour's getting darker
        if(isDecreasing)
        {
            counter--;
            if(counter < 150)
            {
                // if the colour is maximally dark, revert the process by changing the bool
                isDecreasing = false;
            }
        }
        else
        {
            counter++;
            if(counter >= 300)
            {
                // if the colour is maximally light, revert the process by changing the bool
                isDecreasing = true;
            }
        }

        if (counter <= 255)
        {
            // update all the below elements with a new colour based on the counter
            welcomeText.GetComponent<Outline>().effectColor = new Color32(204, 0, (byte)(counter-120), 255);
            welcomeTextLight.color = new Color32(255, 180, (byte)(counter - 100), 120);
            topLogo.color = new Color32((byte)counter, (byte)counter, (byte)counter, 255);
            logoLight1.color = new Color32((byte)counter, (byte)counter, (byte)counter, 120);
            logoLight2.color = new Color32((byte)counter, (byte)counter, (byte)counter, 120);
        }
    }

    // routine for the logo blinking feature
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

    // routine for the logo's colour change
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
