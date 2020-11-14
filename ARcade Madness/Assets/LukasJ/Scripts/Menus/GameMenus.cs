using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenus : MonoBehaviour
{
    public GameObject pauseScreen, optWindow, escWindow;

    public Button optButton, escButton, escCancelButton, resumeButton;
    

    void Start()
    {
        pauseScreen.SetActive(false);
        optWindow.SetActive(false);
        escWindow.SetActive(false);

        optButton.onClick.AddListener(OpenPauseScreen);
        escButton.onClick.AddListener(OpenEscWindow);
        escCancelButton.onClick.AddListener(CloseEscWindow);
        resumeButton.onClick.AddListener(ClosePauseScreen);
    }

    void OpenPauseScreen()
    {
        pauseScreen.SetActive(true);
        OpenOptWindow();
    }

    void ClosePauseScreen()
    {
        pauseScreen.SetActive(false);
    }

    void OpenOptWindow()
    {
        optWindow.SetActive(true);
    }

    void CloseOptWindow()
    {
        optWindow.SetActive(false);
    }

    void OpenEscWindow()
    {
        escWindow.SetActive(true);
        CloseOptWindow();
    }

    void CloseEscWindow()
    {
        escWindow.SetActive(false);
        OpenOptWindow();
    }
}
