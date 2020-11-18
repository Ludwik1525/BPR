using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    private int roomSize;
    private int playerCount;

    private string roomName;

    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text sizeText;


    // for joining a room when clicked on its button
    public void JoinRoomOnClick()
    {
        if (playerCount < roomSize)
        {
            PhotonNetwork.JoinRoom(roomName);
            FindObjectOfType<LobbyController>().ClearRoomListings();
        }
    }

    // setting all the player's information on its button
    public void SetRoom(string nameInput, int sizeInput, int countInput)
    {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        nameText.text = nameInput;
        sizeText.text = countInput + "/" + sizeInput;
    }
}
