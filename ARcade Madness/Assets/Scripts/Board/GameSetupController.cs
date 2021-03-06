﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupController : MonoBehaviour
{
    [SerializeField]
    private int menuSceneIndex;

    [SerializeField]
    private Spawn spawn;

    [SerializeField]
    private Transform[] spawnPositions;

    private ColourPalette colours;

    private PhotonView PV1, PV2, PV3;

    private GameObject player;

    private GameObject score;

    private Route currentRoute;
    private int readyCount;
    private bool startGame = false;
    
    public GameObject infoPanel;
    public GameObject board;

    public static List<GameObject> players = new List<GameObject>();
    public static List<GameObject> newPlayers = new List<GameObject>();


    private void Start()
    {
        currentRoute = FindObjectOfType<Route>();
        spawn = FindObjectOfType<Spawn>();
        colours = FindObjectOfType<ColourPalette>();
        CreatePlayer();
    }

    private void Update()
    {
        if (startGame && infoPanel.activeInHierarchy)
        {
            infoPanel.SetActive(false);
        }
    }

    public void CreatePlayer()
    {
        if (!GameController.gc.doesHavePosition)
        {
            PlayerPrefs.SetInt("MyPowerups", 0);
            GameController.gc.roundCount = 0;
            PlayerPrefs.SetInt("totalPos", 0);
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position, Quaternion.identity);

            ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            thisCurrency.Add("Currency", 0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);
            FindObjectOfType<ScoreInfo>().GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered,
                (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0);

            PlayerPrefs.SetInt("Score", 0);
            FindObjectOfType<ScoreInfo>().GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered,
                (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0);
        }
        else
        {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]]]
            .transform.GetChild(1).GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

            GameController.gc.SetTurns();
            player.transform.rotation = Quaternion.LookRotation(player.transform.position - currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]] + 1].position);
        }

        PV1 = player.GetComponent<PhotonView>();
        PV1.RPC("RPC_AddToList", RpcTarget.AllBuffered);
        PV1.RPC("RPC_SetParent", RpcTarget.AllBuffered);

        PV2 = player.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<PhotonView>();
        if (PV2.IsMine)
        {
            PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]);
        }
    }

    public void DisconnectPlayer()
    {
        if (PhotonNetwork.PlayerList.Length - 1 < 2)
        {
            PV1.RPC("EnableEndScreen", RpcTarget.AllBuffered);
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
        SceneManager.LoadScene(menuSceneIndex);
    }
}