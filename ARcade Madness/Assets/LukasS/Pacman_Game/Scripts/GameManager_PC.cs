using Photon.Pun;
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
    private Transform[] spawnPositions;
    [SerializeField]
    private Button ready_btn;
    [SerializeField]
    private GameObject instruction;
    [SerializeField]
    private GameObject menu;

    private GameObject player;
    private PhotonView playerPV;

    private PhotonView pv;
    private int count = 0;

    public static List<GameObject> playerLoaders;

    private bool start = false;

    private float time = 4f;

    private void Awake()
    {
        playerLoaders = new List<GameObject>();
    }
    void Start()
    {
        instruction.SetActive(true);

        GameObject playerLoader = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerLoadingImg"), menu.transform.position, Quaternion.identity);
        pv = playerLoader.GetComponent<PhotonView>();
        pv.RPC("RPC_SetPlayerLoaderForPacman", RpcTarget.AllBuffered);

        if (pv.IsMine)
        {
            pv.RPC("RPC_SetName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }

        SpawnGhosts();
    }

    // Update is called once per frame
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
                start = true;
            }
        }

        if (start)
        {
            StartCoroutine(CountDown());
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
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_LitBlue"), patrolPoints[Random.Range(0, patrolPoints.Length - 1)].transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_Orange"), patrolPoints[Random.Range(0, patrolPoints.Length - 1)].transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_Pink"), patrolPoints[Random.Range(0, patrolPoints.Length - 1)].transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ghost_Red"), patrolPoints[Random.Range(0, patrolPoints.Length - 1)].transform.position, Quaternion.identity);
        }
    }

    public void SpawnPlayer()
    {
        Vector3 instantiatePosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position;

        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Capsule"), instantiatePosition, Quaternion.identity);
        playerPV = player.GetComponent<PhotonView>();
    }  //"Player_Pacman"

    public void Ready()
    {
        print("c  " + PhotonNetwork.CountOfPlayers);
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
}
