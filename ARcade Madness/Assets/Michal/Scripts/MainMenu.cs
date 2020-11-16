using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, mainMenuObjs, nameBox, optionsMenu, soundsBox, lobby, lobbyObjs, roomCreation, privateRoomBox;

    public GameObject welcomeText, playerNameErrorShort, playerNameErrorLong, playerNameErrorNumbers, roomNameErrorShort, roomNameErrorLong, privateRoomError;

    public InputField playerNameField, roomNameField, privateRoomField;

    public Button acceptNameB, playB, optionsB, quitB, changeNameB, changeVolumeB, acceptVolumeB, optionsBackB, lobbyBackB, roomCreationBackB, roomCreationB, roomAcceptB, players2B, 
        players3B, players4B, privateB, publicB, joinPrivateB, confirmPrivateB, cancelPrivateB;

    private bool isChangingName;

    void Start()
    {
        quitB.onClick.AddListener(QuitApp);
        acceptNameB.onClick.AddListener(SavePlayerName);
        optionsB.onClick.AddListener(OpenOptions);
        optionsBackB.onClick.AddListener(GoBackToMenu);
        changeNameB.onClick.AddListener(OpenNameBox);
        lobbyBackB.onClick.AddListener(GoToMenu);
        roomCreationB.onClick.AddListener(OpenRoomCreation);
        roomCreationBackB.onClick.AddListener(GoBackToLobby);
        playB.onClick.AddListener(GoToLobby);
        roomAcceptB.onClick.AddListener(CreateRoom);
        players2B.onClick.AddListener(Choose2Players);
        players3B.onClick.AddListener(Choose3Players);
        players4B.onClick.AddListener(Choose4Players);
        privateB.onClick.AddListener(SetPrivate);
        publicB.onClick.AddListener(SetPublic);
        joinPrivateB.onClick.AddListener(OpenPrivateRoomBox);
        cancelPrivateB.onClick.AddListener(ClosePrivateRoomBox);
        changeVolumeB.onClick.AddListener(OpenSoundsBox);
        acceptVolumeB.onClick.AddListener(CloseSoundsBox);

        playerNameErrorLong.SetActive(false);
        playerNameErrorShort.SetActive(false);
        playerNameErrorNumbers.SetActive(false);

        optionsMenu.SetActive(false);
        soundsBox.SetActive(false);
        lobby.SetActive(false);
        roomCreation.SetActive(false);

        isChangingName = false;

        if (PlayerPrefs.GetInt("IsAvatarCreated") == 0)
        {
            nameBox.SetActive(true);
            mainMenuObjs.SetActive(false);
        }
        else
        {
            nameBox.SetActive(false);
            mainMenuObjs.SetActive(true);
            welcomeText.GetComponent<Text>().text = "Welcome, " + PlayerPrefs.GetString("NickName");
        }
    }
    
    private void GoToLobby()
    {
        mainMenu.SetActive(false);
        lobby.SetActive(true);
        lobbyObjs.SetActive(true);
        Choose2Players();
    }

    private void GoToMenu()
    {
        mainMenu.SetActive(true);
        mainMenuObjs.SetActive(true);
        lobby.SetActive(false);
    }

    private void OpenRoomCreation()
    {
        roomCreation.SetActive(true);
        lobbyObjs.SetActive(false);

        roomNameField.text = "";

        roomNameErrorLong.SetActive(false);
        roomNameErrorShort.SetActive(false);

        Choose2Players();
        SetPublic();
    }

    private void OpenPrivateRoomBox()
    {
        privateRoomField.text = "";
        privateRoomError.SetActive(false);
        lobbyObjs.SetActive(false);
        privateRoomBox.SetActive(true);
    }

    public void TurnOnPrivateRoomError()
    {
        privateRoomError.SetActive(true);
    }

    public void TurnOffPrivateRoomError()
    {
        privateRoomError.SetActive(false);
    }

    public void ClosePrivateRoomBox()
    {
        lobbyObjs.SetActive(true);
        privateRoomBox.SetActive(false);
    }

    private void GoBackToLobby()
    {
        roomCreation.SetActive(false);
        lobbyObjs.SetActive(true);
    }

    private void OpenOptions()
    {
        optionsMenu.SetActive(true);
        mainMenuObjs.SetActive(false);
    }

    private void GoBackToMenu()
    {
        optionsMenu.SetActive(false);
        mainMenuObjs.SetActive(true);
        FindObjectOfType<LogoBlinker>().enabled = false;
        FindObjectOfType<LogoBlinker>().enabled = true;
    }

    private void OpenNameBox()
    {
        optionsMenu.SetActive(false);
        nameBox.SetActive(true);
        isChangingName = true;
    }

    private void OpenSoundsBox()
    {
        optionsMenu.SetActive(false);
        soundsBox.SetActive(true);
    }

    private void CloseSoundsBox()
    {
        optionsMenu.SetActive(true);
        soundsBox.SetActive(false);
    }

    private void CreateRoom()
    {
        if(IsNameOK(1))
        {
                PlayerPrefs.SetString("RoomName", roomNameField.text);
                FindObjectOfType<LobbyController>().CreateRoom();
                roomCreation.SetActive(false);
                lobbyObjs.SetActive(true);
        }
    }

    private void SetPrivate()
    {
        privateB.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        publicB.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        PlayerPrefs.SetInt("IsPrivate", 1);
    }

    private void SetPublic()
    {
        privateB.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        publicB.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        PlayerPrefs.SetInt("IsPrivate", 0);
    }

    private void Choose2Players()
    {
        players2B.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        players3B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players4B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        PlayerPrefs.SetInt("RoomSize", 2);
    }

    private void Choose3Players()
    {
        players2B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players3B.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        players4B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        PlayerPrefs.SetInt("RoomSize", 3);
    }

    private void Choose4Players()
    {
        players2B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players3B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players4B.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        PlayerPrefs.SetInt("RoomSize", 4);
    }

    private void SavePlayerName()
    {
        if (IsNameOK(0))
        {
            PlayerPrefs.SetInt("IsAvatarCreated", 1);
            PlayerPrefs.SetString("NickName", playerNameField.text);
            FindObjectOfType<LobbyController>().PlayerNameUpdate();
            welcomeText.GetComponent<Text>().text = "Welcome, " + PlayerPrefs.GetString("NickName");
            nameBox.SetActive(false);
            if(isChangingName)
            {
                isChangingName = false;
                optionsMenu.SetActive(true);
            }
            else
            {
                mainMenuObjs.SetActive(true);
            }
        }
    }

    private bool IsNameOK(int option)
    {
        if (option == 0)
        {
            playerNameErrorLong.SetActive(false);
            playerNameErrorShort.SetActive(false);
            playerNameErrorNumbers.SetActive(false);

            if (playerNameField.text.Length < 3)
            {
                playerNameErrorShort.SetActive(true);
                return false;
            }
            else if (playerNameField.text.Length > 7)
            {
                playerNameErrorLong.SetActive(true);
                return false;
            }
            else if(playerNameField.text.Any(char.IsDigit))
            {
                playerNameErrorNumbers.SetActive(true);
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (option == 1)
        {
            roomNameErrorLong.SetActive(false);
            roomNameErrorShort.SetActive(false);

            if (roomNameField.text.Length < 3)
            {
                roomNameErrorShort.SetActive(true);
                return false;
            }
            else if (roomNameField.text.Length > 7)
            {
                roomNameErrorLong.SetActive(true);
                return false;
            }
            else
            {
                return true;
            }
        }
        else return false;
    }

    private void QuitApp()
    {
        Application.Quit();
    }
}
