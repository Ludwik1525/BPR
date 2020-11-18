using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    private PhotonView PV;
    private GameObject rocket;
    private Transform playersParent;

    private void Start()
    {
        rocket = gameObject.transform.GetChild(1).GetChild(1).gameObject;
    }

    //ready for game after placing ar board
    public bool readyForGame;


    [PunRPC]
    void RPC_AddToList()
    {
        PV = GetComponent<PhotonView>();
        
        GameSetupController.players.Add(this.gameObject);
    }


    [PunRPC]
    void RPC_AddToNewList()
    {
        PV = GetComponent<PhotonView>();

        GameSetupController.newPlayers.Add(this.gameObject);
    }

    [PunRPC]
    void RPC_SetParent()
    {
        playersParent = GameObject.Find("PlayersParent").transform;

        this.gameObject.transform.SetParent(playersParent);
    }

    [PunRPC]
    void EnableEndScreen()
    {
        FindObjectOfType<BoardMenus>().TurnOnWinScreen();
    }

    [PunRPC]
    private void EnableRocket()
    {
        rocket.SetActive(true);
    }

    [PunRPC]
    private void DisableRocket()
    {
        rocket.SetActive(false);
    }

}
