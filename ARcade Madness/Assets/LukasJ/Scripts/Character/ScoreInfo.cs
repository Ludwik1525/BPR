using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreInfo : MonoBehaviour
{
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        if(PhotonNetwork.IsMasterClient)
        {
            PV.RPC("ActivateChildren", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void ActivateChildren()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
