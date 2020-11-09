using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreInfo : MonoBehaviour
{
    private PhotonView PV;

    private Transform scoresParent;

    [PunRPC]
    void RPC_SetParent()
    {
        scoresParent = GameObject.Find("Scores").transform;

        this.gameObject.transform.SetParent(scoresParent);
    }
}
