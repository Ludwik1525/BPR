using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class FireballSetupManager : MonoBehaviour
{
    private PhotonView PV, PV2;
    private GameObject player;
    private GameObject spawnPositions;

    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private Button ready_btn;
    private PhotonView imagePV;
    private int count = 0;

    public static List<GameObject> playerLoaders;

    private void Awake()
    {
        playerLoaders = new List<GameObject>();
    }

    void Start()
    {
        instruction.SetActive(true);

        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), content.transform.position, Quaternion.identity);
        imagePV = playerLoader.GetComponent<PhotonView>();
        imagePV.RPC("RPC_SetPlayerLoaderForFireBall", RpcTarget.AllBuffered);

        if (imagePV.IsMine)
        {
            imagePV.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }

        spawnPositions = GameObject.Find("SpawnPositions");
        //SpawnPlayer();
    }

    void Update()
    {
        while (count < playerLoaders.Count)
        {
            foreach (var a in playerLoaders)
            {
                if (a.GetComponent<Loader>().ready == true)
                {
                    count++;
                }
                else
                {
                    count = 0;
                    return;
                }
                print(count);
            }

            if (count == playerLoaders.Count)
            {
                instruction.SetActive(false);
                SpawnPlayer();
            }
        }
    }

    private void SpawnPlayer()
    {
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_FireBall"),
        spawnPositions.transform.GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

        PV = player.GetComponent<PhotonView>();
        PV2 = player.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<PhotonView>();
        if (PV2.IsMine)
        {
            PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]);
        }
        if(PV.IsMine)
        {
            PV.RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    public void Ready()
    {
        print("c  " + PhotonNetwork.CountOfPlayers);
        imagePV.RPC("ReadyIndication", RpcTarget.AllBuffered);
        ready_btn.interactable = false;
    }
}
