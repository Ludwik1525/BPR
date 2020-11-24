using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

public class RoomController : MonoBehaviourPunCallbacks
{
    private bool isStarting = false;

    [SerializeField]
    private int multiplayerSceneIndex;

    private float timerValue = 5;

    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject roomPanel;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject roomTypeButton;
    [SerializeField]
    private GameObject playerListingPrefab;
    [SerializeField]
    private Transform playersContainer;

    [SerializeField]
    private Text roomNameDisplay;
    [SerializeField]
    private Text timer;
    [SerializeField]
    private Text waitText;

    private PhotonView PV;


    // clearing all the players name displayer
    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    // method for listing all the player in the room
    IEnumerator ListPlayers()
    {
        int playerNumber = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerListingImg"), playersContainer.position, Quaternion.identity);
            tempListing.transform.SetParent(playersContainer);
            tempListing.name = player.NickName + "_Displayer";
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;

            // assigning an unique number for each player, to be used in ther scenes etc
            ExitGames.Client.Photon.Hashtable playerNo = new ExitGames.Client.Photon.Hashtable();
            playerNo.Add("PlayerNo", playerNumber);
            player.SetCustomProperties(playerNo);

            playerNumber++;

            yield return new WaitForSeconds(0.1f);
        }

        // delete all the repeating elements
        if (GameObject.FindGameObjectsWithTag("Displayer") != null)
        {
            foreach (GameObject displayer in GameObject.FindGameObjectsWithTag("Displayer"))
            {
                if (displayer.name == "PlayerListingImg(Clone)")
                {
                    Destroy(displayer);
                }
            }
        }

        // enable the start button if there are at least 2 players in the room
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            startButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    // method defining the behaviours of a room and the list of all open rooms when the player is joining one
    public override void OnJoinedRoom()
    {
        PV = GetComponent<PhotonView>();
        string roomType = "";
        if (PhotonNetwork.CurrentRoom.IsVisible) // check the room's privacy settings
        {
            roomType = "public";
        }
        else
        {
            roomType = "private";
        }
        //display the room's name in the top
        roomNameDisplay.text = "Room    " + PhotonNetwork.CurrentRoom.Name + " (" + roomType + ")";
        if (PhotonNetwork.IsMasterClient) // if the player is the host, enable additional functions
        {
            startButton.SetActive(true);
            roomTypeButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
            roomTypeButton.SetActive(false);
        }
        ClearPlayerListings(); // update the list of players by emptying it and creating a new one
        StartCoroutine("ListPlayers");
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);

        // assign a colour for the player
        ExitGames.Client.Photon.Hashtable thisPColour = new ExitGames.Client.Photon.Hashtable();
        thisPColour.Add("ColourID", null);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisPColour);
    }

    // method defining the behaviour of a room when other player is joining one
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPlayerListings();
        StartCoroutine("ListPlayers");
    }

    // method defining the behaviour of a room when other player is leaving one
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerListings();
        StartCoroutine("ListPlayers");
        if (PhotonNetwork.IsMasterClient) // in case the player becomes the host
        {
            startButton.SetActive(true);
            roomTypeButton.SetActive(true);
        }
    }

    // for starting the game
    public void BeginStartingGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                StartCoroutine("CountTime");
                isStarting = true;
                startButton.GetComponent<Button>().interactable = false;
            }
        }
    }

    // for updating the countdown
    [PunRPC]
    void RPC_UpdateTimer()
    {
        isStarting = true;
        waitText.gameObject.SetActive(true);
        timer.gameObject.SetActive(true);
        timer.text = "" + timerValue;
        timerValue--;
    }

    // routine for the countdown
    IEnumerator CountTime()
    {
        while (timerValue >= 0)
        {
            PV.RPC("RPC_UpdateTimer", RpcTarget.AllBuffered);
            yield return new WaitForSeconds(1);
        }
        PV.RPC("RPC_ResetTimer", RpcTarget.AllBuffered);
        StartGame();
    }

    // for resetting the timer
    [PunRPC]
    void RPC_ResetTimer()
    {
        isStarting = false;
        timerValue = 5;
        waitText.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
    }

    // for aborting the start
    [PunRPC]
    void RPC_AbortStarting()
    {
        StopCoroutine("CountTime");
        PV.RPC("RPC_ResetTimer", RpcTarget.AllBuffered);
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    // for calling the game start
    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel("Spinner_Gameplay");
        //PhotonNetwork.LoadLevel("Pacman_Gameplay");

        //PhotonNetwork.LoadLevel("FireBallFightMiniGame");
        //PhotonNetwork.LoadLevel("BoardScene");
    }

    // for joining the lobby backs
    IEnumerator RejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    // for defining the room's behaviour then the player leaves it
    public void BackOnClick()
    {
        ExitGames.Client.Photon.Hashtable thisPColour = new ExitGames.Client.Photon.Hashtable();
        thisPColour.Add("ColourID", null);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisPColour);

        if(isStarting) // abort starting if it is happening
        {
            PV.RPC("RPC_AbortStarting", RpcTarget.AllBuffered);
        }

        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        FindObjectOfType<MainMenu>().ClosePrivateRoomBox();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(RejoinLobby());
    }

    // for changing the room's privacy settings
    public void ChangeRoomType()
    {
        PV.RPC("RPC_ChangeType", RpcTarget.AllBuffered);
    }

    // for updating the room's privacy settings
    [PunRPC]
    void RPC_ChangeType()
    {
        string roomType = "";

        PhotonNetwork.CurrentRoom.IsVisible = !PhotonNetwork.CurrentRoom.IsVisible;

        if (PhotonNetwork.CurrentRoom.IsVisible)
        {
            roomType = "public";
        }
        else
        {
            roomType = "private";
        }
        roomNameDisplay.text = "Room    " + PhotonNetwork.CurrentRoom.Name + " (" + roomType + ")";
    }
}
