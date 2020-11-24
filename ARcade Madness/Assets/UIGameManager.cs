using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public class UIGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Button ready_btn;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    private GameObject mainUI;
    private GameSetupController gsc;

    [SerializeField]
    private List<GameObject> loadersSpawns = new List<GameObject>();

    public static List<GameObject> playerLoaders;

    private PhotonView pv;
    private int count = 0;

    private void Awake()
    {
        playerLoaders = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gsc = FindObjectOfType<GameSetupController>();

        //GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), loadersSpawns[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].transform.position, Quaternion.identity);
        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), menu.transform.position, Quaternion.identity);

        pv = playerLoader.GetComponent<PhotonView>();
        pv.RPC("RPC_SetPlayerLoaderForBoard", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]);
        print((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]);

        if (pv.IsMine)
        {
            pv.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
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
                mainUI.SetActive(true);
                gsc.CreatePlayer();

            }
        }
    }

    public void Ready()
    {
        print("c  " + PhotonNetwork.CountOfPlayers);
        pv.RPC("ReadyIndication", RpcTarget.AllBuffered);
        ready_btn.interactable = false;
    }
}
