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

    private PhotonView PV1, PV2;
    
    private GameObject player;

    private Route currentRoute;
    private int readyCount;
    private bool startGame = false;
    public GameObject infoPanel;

    public static List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        currentRoute = FindObjectOfType<Route>();
        spawn = FindObjectOfType<Spawn>();
        colours = FindObjectOfType<ColourPalette>();
    }

    private void Update()
    {
        if(!startGame)
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<PlayerScript>().readyForGame)
                {
                    readyCount++;
                }
                else
                {
                    readyCount = 0;
                }
            }
            if (readyCount == players.Count)
            {
                print("ready");
                startGame = true;
                infoPanel.SetActive(false);
                CreatePlayer();
            }
        }
       

    }

    private void CreatePlayer()
    {
        if (!GameController.gc.doesHavePosition)
        {
            PlayerPrefs.SetInt("totalPos", 0);
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position, Quaternion.identity);
        }
        else
        {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]]]
            .transform.GetChild(1).GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

            GameController.gc.SetTurns();
            player.transform.rotation = Quaternion.LookRotation(player.transform.position - currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]] + 1].position);
            //player.transform.LookAt(currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]]+1]);
        }
        
        //player.transform.parent = 
        //spawn.AssignSpawnPosition((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

        PV1 = player.GetComponent<PhotonView>();
        PV1.RPC("RPC_AddToList", RpcTarget.AllBuffered);
        PV1.RPC("RPC_SetParent", RpcTarget.AllBuffered);

        PV2 = player.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<PhotonView>();
        if(PV2.IsMine)
        {
            PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]);
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine("DisconnectAndLoad");
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