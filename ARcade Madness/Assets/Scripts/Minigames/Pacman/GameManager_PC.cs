﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_PC : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private TextMeshProUGUI startUi;

    //[SerializeField]
    //private GameObject[] ghosts;
    [SerializeField]
    private GameObject[] patrolPoints;

    [SerializeField]
    private GameObject pointParent;

    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private Button ready_btn;
    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    private GameObject menu;

    private GameObject player;
    private PhotonView playerPV, PV2, myPV;

    [SerializeField]
    private GameObject playersParent;

    [SerializeField]
    private GameObject winScreen;
    private int playersLeft = PhotonNetwork.PlayerList.Length;

    [SerializeField]
    private List<GameObject> loadersSpawns = new List<GameObject>();

    private string[] namesToDisplay;
    private List<string> finalNames;
    private List<int> finalScores;

    private PhotonView pv;
    private int count = 0;

    public static List<GameObject> playerLoaders;

    private bool start = false;

    private float time = 4f;

    private AudioManagerPacman audioManager;


    private void Awake()
    {
        playerLoaders = new List<GameObject>();
        audioManager = FindObjectOfType<AudioManagerPacman>();
    }
    void Start()
    {
        SpawnGhosts();
        SpawnPlayer();

    }

    public void LoadingPanel()
    {
        instruction.SetActive(true);
        StartCoroutine(LoadingCorutine());

    }

    IEnumerator LoadingCorutine()
    {
        yield return new WaitForSeconds(0.3f);


        playersLeft = PhotonNetwork.PlayerList.Length;
        myPV = GetComponent<PhotonView>();
        instruction.SetActive(true);
        finalNames = new List<string>();
        finalScores = new List<int>();
        namesToDisplay = new string[PhotonNetwork.PlayerList.Length];

        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), loadersSpawns[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].transform.localPosition, Quaternion.identity);
        pv = playerLoader.GetComponent<PhotonView>();
        pv.RPC("RPC_SetPlayerLoaderForPacman", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]);

        if (pv.IsMine)
        {
            pv.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }

    }

    // Update is called once per frame
    void Update()
    {
        while (count < PhotonNetwork.PlayerList.Length && playerLoaders.Count > 0)
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

            if (count == PhotonNetwork.PlayerList.Length)
            {
                instruction.SetActive(false);
                start = true;

                
            }
        }

        if (start)
        {
            StartCoroutine(CountDown());
        }

        if (playersLeft <= 0 || pointParent.transform.childCount < 1)
        {
            if (!winScreen.activeInHierarchy)
            {
                winScreen.SetActive(true);
                winScreen.transform.GetChild(1).gameObject.SetActive(false);
                SortPlayersOrder();
                myPV.RPC("PlayScoreboardSound", RpcTarget.AllBuffered);
            }
        }
    }

    IEnumerator CountDown()
    {
        time -= Time.deltaTime;
        if(time > 1)
        {
            startUi.text = $"{Mathf.CeilToInt(time -1)}";
        }
        if (time <= 1)
        {
            startUi.text = "GO!";
            yield return new WaitForSeconds(1);
           
            startUi.enabled = false;
            start = false;

            door.gameObject.SetActive(false);
        }
         
    }


    private void SpawnGhosts()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var g1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_LitBlue"), patrolPoints[0].transform.position, Quaternion.identity);
            g1.GetComponent<PhotonView>().RPC("SetParent", RpcTarget.AllBuffered);
            var g2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_Orange"), patrolPoints[1].transform.position, Quaternion.identity);
            g2.GetComponent<PhotonView>().RPC("SetParent", RpcTarget.AllBuffered);
            var g3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_Pink"), patrolPoints[2].transform.position, Quaternion.identity);
            g3.GetComponent<PhotonView>().RPC("SetParent", RpcTarget.AllBuffered);
            var g4 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_Red"), patrolPoints[3].transform.position, Quaternion.identity);
            g4.GetComponent<PhotonView>().RPC("SetParent", RpcTarget.AllBuffered);
        }
    }

    public void SpawnPlayer()
    {
        Vector3 instantiatePosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position;

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_Pacman"), instantiatePosition, Quaternion.identity);
        playerPV = player.GetComponent<PhotonView>();
        PV2 = player.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<PhotonView>();

        if (PV2.IsMine)
        {
            PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]);
        }

        if (playerPV.IsMine)
        {
            playerPV.RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
            playerPV.RPC("SetMyParent", RpcTarget.AllBuffered);
        }
    } 

    public void Ready()
    {
        pv.RPC("ReadyIndication", RpcTarget.AllBuffered);
        ready_btn.interactable = false;
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

    [PunRPC]
    public void SubstractPlayersLeft()
    {
        playersLeft--;
    }

    public void SortPlayersOrder()
    {
        finalScores.Sort();
        finalScores.Reverse();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            string checker = finalScores[i].ToString();
            for (int j = 0; j < finalNames.Count; j++)
            {
                if (finalNames[j].Contains(checker))
                {
                    namesToDisplay[i] = finalNames[j];
                }
            }
        }

        myPV.RPC("SetFinalScores", RpcTarget.AllBuffered);
        DisplayPowerups();
    }

    [PunRPC]
    void SetFinalScores()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i < 3)
            {
                winScreen.transform.GetChild(2).GetChild(i).gameObject.SetActive(true);

                winScreen.transform.GetChild(2).GetChild(i).GetComponent<Text>().text = (i + 1) + ".  " + namesToDisplay[i] + ", " + (PhotonNetwork.PlayerList.Length - i);
            }
        }

        
    }

    private void DisplayPowerups()
    {
        for (int i = 0; i < namesToDisplay.Length; i++)
        {
            for (int j = 0; j < namesToDisplay.Length; j++)
            {
                if (namesToDisplay[i].Contains(playersParent.transform.GetChild(j).GetComponent<PhotonView>().Owner.NickName))
                {
                    playersParent.transform.GetChild(j).GetComponent<PlayerScript_PC>().placement = i + 1;
                    if (i == 0)
                        playersParent.transform.GetChild(j).GetComponent<PlayerScript_PC>().DisplayScore();
                                            
                    playersParent.transform.GetChild(j).GetComponent<PhotonView>().RPC("DisplayCoins", RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    public void AddMeToLists(string myName, int myScore)
    {
        finalNames.Add(myName + " " + myScore);
        finalScores.Add(myScore);
    }

    [PunRPC]
    private void PlayCoinCollectSound()
    {
        audioManager.PlayCoinCollectSound();
    }

    [PunRPC]
    public void PlayDeathSound()
    {
        audioManager.PlayDeathSound();
    }

    [PunRPC]
    public void PlayScoreboardSound()
    {
        audioManager.PlayScoreboardSound();
    }
}
