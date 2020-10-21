using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    private PhotonView PV;


    [PunRPC]
    void RPC_AddToList()
    {
        PV = GetComponent<PhotonView>();
        
        GameSetupController.players.Add(this.gameObject);
    }
}
