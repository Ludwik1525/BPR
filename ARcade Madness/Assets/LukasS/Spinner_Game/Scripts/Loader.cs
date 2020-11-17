using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update

    private PhotonView PV;

    private Transform playersParent;

    public bool ready = false;


    [PunRPC]
    void RPC_SetParent()
    {
        playersParent = GameObject.Find("Content").transform;

        this.gameObject.transform.SetParent(playersParent);
    }


}
