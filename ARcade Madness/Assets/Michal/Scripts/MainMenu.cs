using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, mainMenuObjs, nameBox, optionsMenu, lobby, lobbyObjs, roomCreation;

    public GameObject welcomeText, playerNameErrorShort, playerNameErrorLong, roomNameErrorShort, roomNameErrorLong, passwordErrorLong;

    public InputField playerNameField, roomNameField, passwordNameField;

    public Button acceptNameB, playB, optionsB, quitB, changeNameB, optionsBackB, lobbyBackB, roomCreationBackB, roomCreationB, roomAcceptB, players2B, players3B, players4B;

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

        playerNameErrorLong.SetActive(false);
        playerNameErrorShort.SetActive(false);

        optionsMenu.SetActive(false);
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
        passwordNameField.text = "";

        roomNameErrorLong.SetActive(false);
        roomNameErrorShort.SetActive(false);
        passwordErrorLong.SetActive(false);
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
    }

    private void OpenNameBox()
    {
        optionsMenu.SetActive(false);
        nameBox.SetActive(true);
        isChangingName = true;
    }

    private void CreateRoom()
    {
        if(IsNameOK(1))
        {
            if (IsNameOK(2))
            {
                PlayerPrefs.SetString("RoomName", roomNameField.text);
                FindObjectOfType<LobbyController>().CreateRoom();
                roomCreation.SetActive(false);
                lobbyObjs.SetActive(true);
            }
        }
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
        else if (option == 2)
        {
            passwordErrorLong.SetActive(false);

            if (passwordNameField.text.Length > 7)
            {
                passwordErrorLong.SetActive(true);
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
