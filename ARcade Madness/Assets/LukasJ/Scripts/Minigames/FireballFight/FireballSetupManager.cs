﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class FireballSetupManager : MonoBehaviour
{
    private PhotonView PV, PV2;
    private GameObject player;
    private GameObject spawnPositions;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        spawnPositions = GameObject.Find("SpawnPos");
    }
    
    void Update()
    {
        
    }

    private void SpawnPlayer()
    {
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_Fireball"),
        spawnPositions.transform.GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);


        PV2 = player.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<PhotonView>();
        if (PV2.IsMine)
        {
            PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]);
        }
    }
}
