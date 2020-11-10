using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreInfo : MonoBehaviour
{
    private PhotonView PV;

    private Transform scoresParent;

    [PunRPC]
    void RPC_SetParent(int index)
    {
        scoresParent = GameObject.Find("Scores").transform;

        this.gameObject.transform.SetParent(scoresParent.transform.GetChild(index));

        this.gameObject.transform.localPosition = Vector3.zero; //new Vector3(0, -20 * (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0);
    }
}
