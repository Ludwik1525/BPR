using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class FireballSetupManager : MonoBehaviour
{
    private int count = 0;

    private GameObject player,  spawnPositions;

    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    private List<GameObject> loadersSpawns = new List<GameObject>();

    public static List<GameObject> playerLoaders;

    [SerializeField]
    private Button ready_btn;

    private PhotonView PV, PV2, imagePV, PV4;

    [SerializeField]
    private GameObject winScreen;
    private int playersLeft = PhotonNetwork.PlayerList.Length;

    private AudioManagerFireball audioManager;

    [SerializeField]
    private GameObject boardPrefab;
    private ARAnchorManager anchorManager;

    private void Awake()
    {
        playerLoaders = new List<GameObject>();
        audioManager = FindObjectOfType<AudioManagerFireball>();

        anchorManager = FindObjectOfType<ARAnchorManager>();
    }

    void Start()
    {
        instruction.SetActive(true);

        // instantiating the player's displayer on the instructions page
        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), loadersSpawns[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].transform.localPosition, Quaternion.identity);
        imagePV = playerLoader.GetComponent<PhotonView>();
        imagePV.RPC("RPC_SetPlayerLoaderForFireBall", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]);

        if (imagePV.IsMine)
        {
            imagePV.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }

        spawnPositions = GameObject.Find("SpawnPositions");

        anchorManager.anchorPrefab = boardPrefab;
        anchorManager.anchorPrefab.transform.position = ArPersistence.anchor.transform.position;



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

        if (playersLeft <= 1)
        {
            if (!winScreen.activeInHierarchy)
            {
                winScreen.SetActive(true);
                PV4.RPC("DisplayScore", RpcTarget.AllBuffered);
                GetComponent<PhotonView>().RPC("PlayScoreboardSound", RpcTarget.AllBuffered);
            }
        }
    }

    // spawning players
    private void SpawnPlayer()
    {
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_FireBall"),
        spawnPositions.transform.GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

        PV = player.GetComponent<PhotonView>();
        PV2 = player.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<PhotonView>();
        PV4 = player.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<PhotonView>();

        if (PV2.IsMine)
        {
            PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColourID"]);
        }
        if(PV.IsMine)
        {
            PV.RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    // function defining the behavious when the player clicks on the "ready" button
    public void Ready()
    {
        print("c  " + PhotonNetwork.CountOfPlayers);
        imagePV.RPC("ReadyIndication", RpcTarget.AllBuffered);
        ready_btn.interactable = false;
    }

    // function called when the player chooses to disconnect from the game
    public void DisconnectPlayer()
    {
        if (PhotonNetwork.PlayerList.Length - 1 < 2)
        {
            PV.RPC("EnableEndScreen", RpcTarget.AllBuffered);
        }

        StartCoroutine("DisconnectAndLoad");
        PlayerPrefs.SetInt("Score", 0);
    }

    // function to disconnect from the game
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

    public void SubstractPlayersLeft()
    {
        playersLeft--;
    }

    [PunRPC]
    public void PlayFireballThrowSound()
    {
        audioManager.PlayFireballThrowSound();
    }

    [PunRPC]
    private void PlayFireballExplodeSound()
    {
        audioManager.PlayFireballExplodeSound();
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
    
    public void SwitchShieldSounds(bool condition)
    {
        if (condition)
        {
            audioManager.TurnOnShieldSounds();
        }
        else
        {
            audioManager.TurnOffShieldSounds();
        }
    }

}
