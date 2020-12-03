using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // all pages / menus
    public GameObject mainMenu, mainMenuObjs, nameBox, optionsMenu, soundsBox, lobby, 
        lobbyObjs, roomCreation, privateRoomBox, instructionsMenu, instructionsGeneral, instructionsPowerups;

    // all text elements
    public GameObject welcomeText, playerNameErrorShort, playerNameErrorLong, playerNameErrorNumbers, roomNameErrorShort, roomNameErrorLong, privateRoomError;

    // all input fields
    public InputField playerNameField, roomNameField, privateRoomField;

    // all buttons
    public Button acceptNameB, playB, optionsB, quitB, changeNameB, changeVolumeB, acceptVolumeB, optionsBackB, lobbyBackB, roomCreationBackB, roomCreationB, roomAcceptB, players2B, 
        players3B, players4B, privateB, publicB, joinPrivateB, confirmPrivateB, cancelPrivateB, openInstrB, instrBack1B, instrBack2B, instrBack4B, instrGeneralB, instrPowerupsB;

    private bool isChangingName;


    void Start()
    {
        // buttons assignments
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
        openInstrB.onClick.AddListener(OpenInstructionsMenu);
        instrBack1B.onClick.AddListener(OpenOptions);
        instrBack2B.onClick.AddListener(BackToInstructionsMenu);
        instrBack4B.onClick.AddListener(BackToInstructionsMenu);
        instrGeneralB.onClick.AddListener(OpenGeneralInstrMenu);
        instrPowerupsB.onClick.AddListener(OpenPowerupsInstrMenu);

        // turn off all the error message by default
        playerNameErrorLong.SetActive(false);
        playerNameErrorShort.SetActive(false);
        playerNameErrorNumbers.SetActive(false);

        // turn off all the menus except for the main one by default
        optionsMenu.SetActive(false);
        soundsBox.SetActive(false);
        lobby.SetActive(false);
        roomCreation.SetActive(false);

        isChangingName = false;

        // check if the player opens the game for the first time
        if (PlayerPrefs.GetInt("IsAvatarCreated") == 0)
        {
            // if so, open the name box first and let the player choose their name
            nameBox.SetActive(true);
            mainMenuObjs.SetActive(false);
        }
        else
        {
            // if the player has launched the app before, open it normally
            nameBox.SetActive(false);
            mainMenuObjs.SetActive(true);
            welcomeText.GetComponent<Text>().text = "Welcome, " + PlayerPrefs.GetString("NickName");
        }
    }
    
    // opening the lobby's canvas
    private void GoToLobby()
    {
        mainMenu.SetActive(false);
        lobby.SetActive(true);
        lobbyObjs.SetActive(true);
        Choose2Players();
    }

    // opening the main menu
    private void GoToMenu()
    {
        mainMenu.SetActive(true);
        mainMenuObjs.SetActive(true);
        lobby.SetActive(false);
    }

    // opening the menu for creating a new room
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

    // opening the prompt for joining a private room
    private void OpenPrivateRoomBox()
    {
        privateRoomField.text = "";
        privateRoomError.SetActive(false);
        lobbyObjs.SetActive(false);
        privateRoomBox.SetActive(true);
    }

    // turning on an error while connecting to a private room
    public void TurnOnPrivateRoomError()
    {
        privateRoomError.SetActive(true);
    }

    // turning off an error while connecting to a private room
    public void TurnOffPrivateRoomError()
    {
        privateRoomError.SetActive(false);
    }

    // closing the prompt for joining a private room
    public void ClosePrivateRoomBox()
    {
        lobbyObjs.SetActive(true);
        privateRoomBox.SetActive(false);
    }

    // going back to the lobby from the room's creation menu
    private void GoBackToLobby()
    {
        roomCreation.SetActive(false);
        lobbyObjs.SetActive(true);
    }

    // opening the options
    private void OpenOptions()
    {
        optionsMenu.SetActive(true);
        mainMenuObjs.SetActive(false);
        instructionsMenu.SetActive(false);
    }

    // going back to the main menu canvas
    private void GoBackToMenu()
    {
        optionsMenu.SetActive(false);
        mainMenuObjs.SetActive(true);
        FindObjectOfType<LogoBlinker>().enabled = false;
        FindObjectOfType<LogoBlinker>().enabled = true;
    }

    // opening the name change prompt
    private void OpenNameBox()
    {
        optionsMenu.SetActive(false);
        nameBox.SetActive(true);
        isChangingName = true;
    }

    // opening the volume prompt
    private void OpenSoundsBox()
    {
        optionsMenu.SetActive(false);
        soundsBox.SetActive(true);
    }

    // closing the volume prompt
    private void CloseSoundsBox()
    {
        optionsMenu.SetActive(true);
        soundsBox.SetActive(false);
    }

    // creating a room
    private void CreateRoom()
    {
        if(IsNameOK(1)) // if the name matches the criteria
        {
                PlayerPrefs.SetString("RoomName", roomNameField.text);
                FindObjectOfType<LobbyController>().CreateRoom();
                roomCreation.SetActive(false);
                lobbyObjs.SetActive(true);
        }
    }

    // opening instructions menu
    private void OpenInstructionsMenu()
    {
        instructionsMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    // opening general instructions menu
    private void OpenGeneralInstrMenu()
    {
        instructionsMenu.SetActive(false);
        instructionsGeneral.SetActive(true);
    }

    // opening power-ups instructions menu
    private void OpenPowerupsInstrMenu()
    {
        instructionsMenu.SetActive(false);
        instructionsPowerups.SetActive(true);
    }

    // going back to instructions menu
    private void BackToInstructionsMenu()
    {
        instructionsMenu.SetActive(true);
        instructionsGeneral.SetActive(false);
        instructionsPowerups.SetActive(false);
    }

    // setting the room to private while creating a room
    private void SetPrivate()
    {
        privateB.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        publicB.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        PlayerPrefs.SetInt("IsPrivate", 1);
    }

    // setting the room to public while creating a room
    private void SetPublic()
    {
        privateB.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        publicB.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        PlayerPrefs.SetInt("IsPrivate", 0);
    }

    // setting the room's size to 2 while creating a room
    private void Choose2Players()
    {
        players2B.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        players3B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players4B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        PlayerPrefs.SetInt("RoomSize", 2);
    }

    // setting the room's size to 3 while creating a room
    private void Choose3Players()
    {
        players2B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players3B.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        players4B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        PlayerPrefs.SetInt("RoomSize", 3);
    }

    // setting the room's size to 4 while creating a room
    private void Choose4Players()
    {
        players2B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players3B.GetComponent<Image>().color = new Color32(250, 50, 25, 255);
        players4B.GetComponent<Image>().color = new Color32(180, 240, 70, 255);
        PlayerPrefs.SetInt("RoomSize", 4);
    }

    // save the player's new name
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

    // method for checking if a name matches the criteria
    private bool IsNameOK(int option)
    {
        if (option == 0) // for the player's name
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
        else if (option == 1) // for the room's name
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

    // closing the application
    private void QuitApp()
    {
        Application.Quit();
    }
}
