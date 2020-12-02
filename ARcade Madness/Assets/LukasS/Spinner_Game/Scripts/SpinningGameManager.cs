using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SpinningGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Button ready_btn;
    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private GameObject battleArenaGameobject;
    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    private GameObject winScreen;


    private GameObject player;
    private PhotonView playerPV;

    [SerializeField]
    private List<GameObject> loadersSpawns = new List<GameObject>();

    public static List<GameObject> playerLoaders;

    private PhotonView pv;

    public List<string> players = new List<string>();


    private int count = 0;

    private int playersLeft = PhotonNetwork.PlayerList.Length;

    private void Awake()
    {
        playerLoaders = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        instruction.SetActive(true);

        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), loadersSpawns[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].transform.localPosition, Quaternion.identity);

        pv = playerLoader.GetComponent<PhotonView>();
        pv.RPC("RPC_SetPlayerLoaderForSpinner", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]);

        if(pv.IsMine)
        {
            pv.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
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
            }

            if (count == playerLoaders.Count)
            {
                instruction.SetActive(false);
                SpawnPlayer();
            }
        }


        //print(player.GetComponent<BattleScript>().placement);

        if (playersLeft == 1)
        {
          if(!winScreen.activeInHierarchy)
            {
                if(!players.Contains(PhotonNetwork.LocalPlayer.NickName))
                {
                    playerPV.RPC("AddMeToListLast", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
                }

                if(PhotonNetwork.IsMasterClient)
                {
                    playerPV.RPC("DisplayScore", RpcTarget.AllBuffered);
                }

                //DisplayScore();
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
        Vector3 instantiatePosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position;

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_Spinner"), instantiatePosition, Quaternion.identity);
        playerPV = player.GetComponent<PhotonView>();

        if (playerPV.IsMine)
        {
            playerPV.RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    public void DisconnectPlayer()
    {
        if (PhotonNetwork.PlayerList.Length - 1 < 2)
        {
            playerPV.RPC("EnableEndScreen", RpcTarget.AllBuffered);
        }

        StartCoroutine("DisconnectAndLoad");
        PlayerPrefs.SetInt("Score", 0);
    }

    IEnumerator DisconnectAndLoad()
    {
        PlayerPrefs.SetInt("totalPos", 0);
        GameController.gc.doesHavePosition = false;
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }

    public int GetPlayersLeft()
    {
        return playersLeft;
    }

    public void SubstuctPlayersLeft()
    {
        playersLeft--;
    }




}
