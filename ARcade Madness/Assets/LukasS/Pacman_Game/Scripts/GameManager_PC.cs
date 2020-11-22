using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_PC : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private TextMeshProUGUI startUi;

    [SerializeField]
    private GameObject[] ghosts;
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
                StartGame();
            }
        }

        //if (start)
        //{
        //    StartCoroutine(CountDown());
        //}
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

    public void StartGame()
    {
        start = true;
    }

    private void SpawnGhosts()
    {
        foreach(var ghost in ghosts)
        {
            int random = Random.Range(0, patrolPoints.Length -1);

            Instantiate(ghost, patrolPoints[random].transform.position, Quaternion.identity);
        }
    }

    public void SpawnPlayer()
    {
        Vector3 instantiatePosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_Pacman"), instantiatePosition, Quaternion.identity);
    }

    public void Ready()
    {
        print("c  " + PhotonNetwork.CountOfPlayers);
        pv.RPC("ReadyIndication", RpcTarget.AllBuffered);
        ready_btn.interactable = false;
    }
}
