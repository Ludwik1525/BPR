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
    void RPC_SetPlayerLoaderForSpinner(int index)
    {
        //Set parent
        playersParent = GameObject.Find("Content").transform.GetChild(index);
        this.gameObject.transform.SetParent(playersParent);

        //Add player to the list
        SpinningGameManager.playerLoaders.Add(this.gameObject);
    } 
    
    [PunRPC]
    void RPC_SetPlayerLoaderForFireBall(int index)
    {
        //Set parent
        playersParent = GameObject.Find("Content").transform.GetChild(index);
        this.gameObject.transform.SetParent(playersParent);

        //Add player to the list
        FireballSetupManager.playerLoaders.Add(this.gameObject);
    }

    [PunRPC]
    void RPC_SetPlayerLoaderForPacman(int index)
    {
        //Set parent
        playersParent = GameObject.Find("Content").transform.GetChild(index);
        this.gameObject.transform.SetParent(playersParent);

        //Add player to the list
        GameManager_PC.playerLoaders.Add(this.gameObject);
    }

    [PunRPC]
    void RPC_SetName(string name)
    {
        playerName.text = name;
    }

    [PunRPC]
    void ReadyIndication()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        ready = true;
    }
}
