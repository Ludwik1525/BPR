using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    private bool isPrivate;

    private int roomSize;

    private string roomName;

    [SerializeField]
    private GameObject lobbyConnectButton;
    [SerializeField]
    private GameObject roomListingPrefab;

    [SerializeField]
    private Transform roomsContainer;

    [SerializeField]
    private Button connectButton;
    [SerializeField]
    private Button refreshButton;

    [SerializeField]
    private InputField nameInput;

    private List<RoomInfo> roomListings;


    // when the player connects to the master server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("NickName"); // get my name from player prefs
        PhotonNetwork.AutomaticallySyncScene = true; // enable automatical synchronization
        lobbyConnectButton.SetActive(true); // enable the button to connect to the lobby
        roomListings = new List<RoomInfo>(); // create a new list of open rooms
    }

    // updating name
    public void PlayerNameUpdate()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
    }

    // joining lobby
    public void JoinLobbyOnClick()
    {
        PhotonNetwork.JoinLobby();
    }

    // destroy all the rooms from the open rooms displayer
    public void ClearRoomListings()
    {
        for (int i = roomsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(roomsContainer.GetChild(i).gameObject);
        }
    }

    // method for updating the list of open rooms by emptying it and filling back
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int tempIndex;
        foreach(RoomInfo room in roomList)
        {
            if(roomListings != null) // if a room is not null, find its index by its name
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }

            if(tempIndex != -1) // if a room is not null
            {
                if (roomsContainer.childCount > tempIndex) // if it is contained in the list, delete it from there
                {
                    roomListings.RemoveAt(tempIndex);
                    Destroy(roomsContainer.GetChild(tempIndex).gameObject);
                }
            }

            if(room.PlayerCount > 0) // if there are any players in the room, add it back
            {   
                roomListings.Add(room);
                ListRoom(room);
            }
            
            int noRooms = 0;

            // loop counting how many rooms with the same are there
            for (int i = 0; i < roomsContainer.transform.childCount; i++) 
            {
                if(room.Name == roomsContainer.transform.GetChild(i).GetChild(0).GetComponent<Text>().text)
                {
                    noRooms++;
                }
            }

            // delete all the rooms with the same name except for the last one (so only one appears in the dispalyer in the end)
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

    // method for finding a room's index by its name
    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    // method for adding a room to the displayer of open rooms
    void ListRoom(RoomInfo room)
    {
        bool isDoubled = false;
        if(room.IsOpen && room.IsVisible)
        {
            for(int i = 0; i < roomsContainer.childCount; i++) // loop for checking if there are more than 1 rooms with the same name
            {
                if (roomsContainer.GetChild(i).GetChild(0).GetComponent<Text>().text == room.Name)
                {
                    isDoubled = true;
                }
            }

            if(!isDoubled) // check to creating a room only once
            {
                GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
                RoomButton tempButton = tempListing.GetComponent<RoomButton>();
                tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
            }
        }
    }

    // method for creating a room
    public void CreateRoom()
    {
        Debug.Log("Creating room now");

        // getting the room's parameters
        roomName = PlayerPrefs.GetString("RoomName");
        roomSize = PlayerPrefs.GetInt("RoomSize");

        // getting the privacy settings
        if(PlayerPrefs.GetInt("IsPrivate") == 1)
        {
            isPrivate = true;
        }
        else
        {
            isPrivate = false;
        }

        // creating a room on the server with given options
        RoomOptions roomOps = new RoomOptions() { IsVisible = !isPrivate, IsOpen = true, MaxPlayers = (byte)roomSize};
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    // show message when the room couldn't be created
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
    }

    // for leaving the lobby
    public void MatchmakingCancel()
    {
        ClearRoomListings();
        PhotonNetwork.LeaveLobby();
    }

    // for joining a room
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(nameInput.text);
        StartCoroutine(WaitForError());
    }

    // routine for trying to connect to a private room
    private IEnumerator WaitForError()
    {
        FindObjectOfType<MainMenu>().TurnOffPrivateRoomError(); // turn off the error message
        yield return new WaitForSeconds(1f); // wait for the connection (the system is trying to find the room now)
        FindObjectOfType<MainMenu>().TurnOnPrivateRoomError(); // if the connection doesn't happen, display an error message
    }

    // method for refreshing the lobby
    public void RefreshLobby()
    {
        ClearRoomListings();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby();
    }
}