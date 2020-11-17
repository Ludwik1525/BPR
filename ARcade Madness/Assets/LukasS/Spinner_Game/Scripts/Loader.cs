using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update

    private PhotonView PV;

    private Transform playersParent;

    public bool ready = false;

    [SerializeField]
    private Text playerName;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    void RPC_SetPlayerLoader()
    {
        //Set parent
        playersParent = GameObject.Find("Content").transform;
        this.gameObject.transform.SetParent(playersParent);

        //Set name
        //playerName.text = PhotonNetwork.NickName;

        //Add player to the list
        SpinningGameManager.playerLoaders.Add(this.gameObject);
    }

    //[PunRPC]
    //void RPC_SetParent()
    //{
    //    playersParent = GameObject.Find("Content").transform;

    //    this.gameObject.transform.SetParent(playersParent);
    //}

    [PunRPC]
    void RPC_SetName()
    {
        playerName.text = PhotonNetwork.NickName;
    }

    [PunRPC]
    void ReadyIndication()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        ready = true;
    }

    //[PunRPC]
    //void RPC_AddPlayerLoaderToList()
    //{
    //    SpinningGameManager.playerLoaders.Add(this.gameObject);
    //}

}
