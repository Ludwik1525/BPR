using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            this.gameObject.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    [PunRPC]
    void SetScore(int index, int score)
    {
        this.gameObject.transform.GetChild(index).GetChild(0).GetChild(2).GetComponent<Text>().text = "" + score;
    }
}
