using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    private PhotonView PV;
    
    private Transform playersParent;


    //ready for game after placing ar board
    public bool readyForGame;


    [PunRPC]
    void RPC_AddToList()
    {
        PV = GetComponent<PhotonView>();
        
        GameSetupController.players.Add(this.gameObject);
    }

    [PunRPC]
    void RPC_SetParent()
    {
        playersParent = GameObject.Find("PlayersParent").transform;

        this.gameObject.transform.SetParent(playersParent);
    }

}
