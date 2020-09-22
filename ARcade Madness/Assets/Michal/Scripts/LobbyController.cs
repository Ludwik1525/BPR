using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyConnectButton;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject mainPanel;

    private string roomName;
    private int roomSize;
    private bool isPrivate;

    private List<RoomInfo> roomListings;
    [SerializeField]
    private Transform roomsContainer;
    [SerializeField]
    private GameObject roomListingPrefab;

    [SerializeField]
    private InputField nameInput;
    [SerializeField]
    private Button connectButton;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectButton.SetActive(true);
        roomListings = new List<RoomInfo>();
    }

    public void PlayerNameUpdate()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
    }

    public void JoinLobbyOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public void ClearRoomListings()
    {
        for (int i = roomsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(roomsContainer.GetChild(i).gameObject);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int tempIndex;
        foreach(RoomInfo room in roomList)
        {
            if(roomListings != null)
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
            if(tempIndex != -1)
            {
                roomListings.RemoveAt(tempIndex);
                if (roomsContainer.childCount > tempIndex)
                {
                    Destroy(roomsContainer.GetChild(tempIndex).gameObject);
                }
            }
            if(room.PlayerCount > 0)
            {   
                roomListings.Add(room);
                ListRoom(room);
            }
            
            int noRooms = 0;

            for(int i = 0; i < roomsContainer.transform.childCount; i++)
            {
                if(room.Name == roomsContainer.transform.GetChild(i).GetChild(0).GetComponent<Text>().text)
                {
                    noRooms++;
                }
            }

            for (int i = 0; i < roomsContainer.transform.childCount; i++)
            {
                if (room.Name == roomsContainer.transform.GetChild(i).GetChild(0).GetComponent<Text>().text)
                {
                    noRooms--;
                    if (noRooms > 1)
                    {
                        Destroy(roomsContainer.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room)
    {
        if(room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public void CreateRoom()
    {
        Debug.Log("Creating room now");

        roomName = PlayerPrefs.GetString("RoomName");
        roomSize = PlayerPrefs.GetInt("RoomSize");

        if(PlayerPrefs.GetInt("IsPrivate") == 1)
        {
            isPrivate = true;
        }
        else
        {
            isPrivate = false;
        }

        RoomOptions roomOps = new RoomOptions() { IsVisible = !isPrivate, IsOpen = true, MaxPlayers = (byte)roomSize};
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
    }

    public void MatchmakingCancel()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(nameInput.text);
        StartCoroutine(WaitForError());
    }

    private IEnumerator WaitForError()
    {
        FindObjectOfType<MainMenu>().TurnOffPrivateRoomError();
        yield return new WaitForSeconds(1f);
        FindObjectOfType<MainMenu>().TurnOnPrivateRoomError();
    }
}