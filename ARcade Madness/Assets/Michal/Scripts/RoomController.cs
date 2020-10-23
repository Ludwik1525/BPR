using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex;

    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private GameObject roomTypeButton;

    [SerializeField]
    private Transform playersContainer;
    [SerializeField]
    private GameObject playerListingPrefab;

    [SerializeField]
    private Text roomNameDisplay;

    private float timerValue = 5;

    [SerializeField]
    private Text timer;

    [SerializeField]
    private Text waitText;

    private PhotonView PV;

    private bool isStarting = false;


    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

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

            ExitGames.Client.Photon.Hashtable playerNo = new ExitGames.Client.Photon.Hashtable();
            playerNo.Add("PlayerNo", playerNumber);
            player.SetCustomProperties(playerNo);

            playerNumber++;

            yield return new WaitForSeconds(0.1f);
        }

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

        if (PhotonNetwork.PlayerList.Length > 1)
        {
            startButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public override void OnJoinedRoom()
    {
        PV = GetComponent<PhotonView>();
        string roomType = "";
        if (PhotonNetwork.CurrentRoom.IsVisible)
        {
            roomType = "public";
        }
        else
        {
            roomType = "private";
        }
        roomNameDisplay.text = "Room    " + PhotonNetwork.CurrentRoom.Name + " (" + roomType + ")";
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            roomTypeButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
            roomTypeButton.SetActive(false);
        }
        ClearPlayerListings();
        StartCoroutine("ListPlayers");
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);

        ExitGames.Client.Photon.Hashtable thisPColour = new ExitGames.Client.Photon.Hashtable();
        thisPColour.Add("ColourID", null);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisPColour);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPlayerListings();
        StartCoroutine("ListPlayers");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerListings();
        StartCoroutine("ListPlayers");
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            roomTypeButton.SetActive(true);
        }
    }

    public void BeginStartingGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                StartCoroutine("CountTime");
                isStarting = true;
            }
        }
    }

    [PunRPC]
    void RPC_UpdateTimer()
    {
        isStarting = true;
        waitText.gameObject.SetActive(true);
        timer.gameObject.SetActive(true);
        timer.text = "" + timerValue;
        timerValue--;
    }

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

    [PunRPC]
    void RPC_ResetTimer()
    {
        isStarting = false;
        timerValue = 5;
        waitText.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
    }

    [PunRPC]
    void RPC_AbortStarting()
    {
        StopCoroutine("CountTime");
        PV.RPC("RPC_ResetTimer", RpcTarget.AllBuffered);
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    IEnumerator RejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick()
    {
        ExitGames.Client.Photon.Hashtable thisPColour = new ExitGames.Client.Photon.Hashtable();
        thisPColour.Add("ColourID", null);
        PhotonNetwork.LocalPlayer.SetCustomProperties(thisPColour);

        if(isStarting)
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

    public void ChangeRoomType()
    {
        PV.RPC("RPC_ChangeType", RpcTarget.AllBuffered);
    }

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
