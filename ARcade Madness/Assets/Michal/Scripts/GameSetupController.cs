﻿using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupController : MonoBehaviour
{
    [SerializeField]
    private int menuSceneIndex;

    [SerializeField]
    private Spawn spawn;

    private ColourPalette colours;

    private PhotonView PV;

    private GameObject player;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        spawn = FindObjectOfType<Spawn>();
        colours = FindObjectOfType<ColourPalette>();
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        if (PV.IsMine)
        {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
                spawn.AssignSpawnPosition((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);
        }
    }

    [PunRPC]
    void RPC_AssignColour()
    {
        if(PV.IsMine)
        {
            player.GetComponentInChildren<SkinnedMeshRenderer>().material = colours.colours[(int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]];
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine("DisconnectAndLoad");
    }

    IEnumerator DisconnectAndLoad()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(menuSceneIndex);
    }
}