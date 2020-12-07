using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardMenus : MonoBehaviour
{
    public GameObject pauseScreen, optWindow, escWindow, mainScreen, winScreen, arScreen;

    public Button optButton, escButton, escCancelButton, resumeButton;
    

    void Start()
    {
        pauseScreen.SetActive(false);
        optWindow.SetActive(false);
        escWindow.SetActive(false);
        winScreen.SetActive(false);
 

        optButton.onClick.AddListener(OpenPauseScreen);
        escButton.onClick.AddListener(OpenEscWindow);
        escCancelButton.onClick.AddListener(CloseEscWindow);
        resumeButton.onClick.AddListener(ClosePauseScreen);
    }

    void OpenPauseScreen()
    {
        pauseScreen.SetActive(true);
        mainScreen.SetActive(false);
        OpenOptWindow();
    }

    void ClosePauseScreen()
    {
        pauseScreen.SetActive(false);
        mainScreen.SetActive(true);
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

    public void TurnOnWinScreen()
    {
        mainScreen.SetActive(true);
        FindObjectOfType<ScoreInfo>().SortPlayersOrder();
        winScreen.SetActive(true);
        mainScreen.SetActive(false);
        escWindow.SetActive(false);
        optWindow.SetActive(false);
    }
}
