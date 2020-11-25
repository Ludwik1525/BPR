using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

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
    public GameObject boardPlayerPrefab;
    public GameObject board;

    public static List<GameObject> players = new List<GameObject>();
    public static List<GameObject> newPlayers = new List<GameObject>();

    public enum RaiseEventCodes
    {
        PlayerSpawnEvenCode = 0
    }

    private void Start()
    {
        //currentRoute = FindObjectOfType<Route>();
        //spawn = FindObjectOfType<Spawn>();
        //colours = FindObjectOfType<ColourPalette>();
        //CreatePlayer();

        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    // Instatiate o ur player on remote 
    void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEvenCode)
        {
            // Getting data sent by events
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];

            GameObject remotePlayer = Instantiate(boardPlayerPrefab, receivedPosition + board.transform.position, receivedRotation);
            PhotonView pv = player.GetComponent<PhotonView>();
            pv.ViewID = (int)data[2];

        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void Update()
    {
        if(startGame && infoPanel.activeInHierarchy)
        {
            infoPanel.SetActive(false);
        }
    }

    public void CreatePlayer()
    {
        if (!GameController.gc.doesHavePosition)
        {
            //GameController.gc.roundCount = 0;
            //PlayerPrefs.SetInt("totalPos", 0);
            //player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            //spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position, Quaternion.identity);

            //ExitGames.Client.Photon.Hashtable thisCurrency = new ExitGames.Client.Photon.Hashtable();
            //thisCurrency.Add("Currency", 0);
            //PhotonNetwork.LocalPlayer.SetCustomProperties(thisCurrency);
            //FindObjectOfType<ScoreInfo>().GetComponent<PhotonView>().RPC("SetCurrency", RpcTarget.AllBuffered,
            //    (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0);

            //PlayerPrefs.SetInt("Score", 0);
            //FindObjectOfType<ScoreInfo>().GetComponent<PhotonView>().RPC("SetScore", RpcTarget.AllBuffered, 
            //    (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"], 0);



            // Set round and total pos to 0
            GameController.gc.roundCount = 0;
            PlayerPrefs.SetInt("totalPos", 0);

            // Get spawn position
            Vector3 spawnPosition = spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position;

            player = Instantiate(boardPlayerPrefab, spawnPosition, Quaternion.identity);
            PV1 = player.GetComponent<PhotonView>();

            //This will create and assign a new unique viewID to this player's photon view
            if(PhotonNetwork.AllocateViewID(PV1))
            {
                object[] data = new object[]
                {
                    player.transform.position - board.transform.position, player.transform.rotation, PV1.ViewID
                };

                // Will be sent to all players but me
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };

                //Raise Events
                PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEvenCode, data, raiseEventOptions, sendOptions);

            }
            //If we fail to allocate viewID we destroy this object
            else
            {
                Destroy(player);
            }

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
            //player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            //currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]]]
            //.transform.GetChild(1).GetChild((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

            //GameController.gc.SetTurns();
            //player.transform.rotation = Quaternion.LookRotation(player.transform.position 
            //- currentRoute.childNodeList[GameController.gc.currentPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]] + 1]
            //.position);
        }

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
        if(PhotonNetwork.PlayerList.Length - 1 < 2)
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