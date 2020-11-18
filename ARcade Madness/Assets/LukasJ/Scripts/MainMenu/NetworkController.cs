using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // try to connect on start
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // display a message when connected
    public override void OnConnectedToMaster()
    {
        Debug.Log("You are now connected to the " + PhotonNetwork.CloudRegion + " server");
    }
}
