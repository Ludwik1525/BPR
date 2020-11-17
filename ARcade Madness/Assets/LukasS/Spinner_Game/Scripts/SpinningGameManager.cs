using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public class SpinningGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Button ready_btn;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private GameObject battleArenaGameobject;
    [SerializeField]
    private GameObject instruction;

    public static List<GameObject> playerLoaders = new List<GameObject>();

    private PhotonView pv;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), menu.transform.position, Quaternion.identity);
        pv = playerLoader.GetComponent<PhotonView>();
        pv.RPC("RPC_SetPlayerLoader", RpcTarget.AllBuffered);

        if(pv.IsMine)
        {
            pv.RPC("RPC_SetName", RpcTarget.AllBuffered);
        }
    }

    void Update()
    {
        while(count < playerLoaders.Count)
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
                print("ready");
                instruction.SetActive(false);
                SpawnPlayer();
            }
        }
        

    }

    public void Ready()
    {
        print("c  " + PhotonNetwork.CountOfPlayers);
        pv.RPC("ReadyIndication", RpcTarget.AllBuffered);
        ready_btn.interactable = false;
    }

    public void SpawnPlayer()
    {
        int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);

        Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_Spinner"), instantiatePosition, Quaternion.identity);
    }

}
