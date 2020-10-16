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
    private Transform playersContainer;
    [SerializeField]
    private GameObject playerListingPrefab;

    [SerializeField]
    private Text roomNameDisplay;

    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    IEnumerator ListPlayers()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerListingImg"), playersContainer.position, Quaternion.identity);
            tempListing.transform.SetParent(playersContainer);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void OnJoinedRoom()
    {
        roomNameDisplay.text = "Room " + PhotonNetwork.CurrentRoom.Name;
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
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
        }
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
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

        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        FindObjectOfType<MainMenu>().ClosePrivateRoomBox();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(RejoinLobby());
    }
}
