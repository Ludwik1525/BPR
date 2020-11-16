using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.IO;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] playerPrefabs;
    public GameObject playerPrefab;

    public Transform[] spawnPositions;

    public GameObject battleArenaGameobject;

    //public enum RaiseEventCodes
    //{
    //    PlayerSpawnEventCode = 0
    //}
    // Start is called before the first frame update
    void Awake()
    {
        //PhotonNetwork.NetworkingClient.EventReceived += OnEvent;

        int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);

        Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;

        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player_Spinner"), instantiatePosition, Quaternion.identity);
        //player.GetComponent<Rigidbody>().
        PhotonView _photonView = player.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnDestroy()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    //}

    //void OnEvent(EventData photonEvent)
    //{
    //    if (photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode)
    //    {
    //        object[] data = (object[])photonEvent.CustomData;
    //        Vector3 receivedPosition = (Vector3)data[0];
    //        Quaternion receivedRotation = (Quaternion)data[1];
    //        //int receivedPlayerSelectionData = (int)data[3];

    //        //GameObject player = Instantiate(playerPrefab, receivedPosition + battleArenaGameobject.transform.position, receivedRotation);
    //        //GameObject player = PhotonNetwork.Instantiate(Path.GetFileName(playerPrefab.name), receivedPosition + battleArenaGameobject.transform.position, receivedRotation);
    //        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","Player1_Attacker"), receivedPosition + battleArenaGameobject.transform.position, receivedRotation);
    //        PhotonView _photonView = player.GetComponent<PhotonView>();
    //        _photonView.ViewID = (int)data[2];
    //    }
    //}

    //public void JoinRoom()
    //{
    //    SpawnPlayer();
    //}

    //private void SpawnPlayer()
    //{
    //    int randomSpawnPoint = Random.Range(0, spawnPositions.Length - 1);

    //    Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;

    //    GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player1_Attacker"), instantiatePosition , Quaternion.identity);
    //    PhotonView _photonView = player.GetComponent<PhotonView>();

        //GameObject playerGameobject2 = Instantiate(playerPrefab, instantiatePosition, Quaternion.identity);

        ////GameObject playerGameobject = PhotonNetwork.Instantiate(Path.Combine();


        //PhotonView _photonView = playerGameobject.GetComponent<PhotonView>();

        //if (PhotonNetwork.AllocateViewID(_photonView))
        //{
        //    object[] data = new object[]
        //    {
        //            playerGameobject.transform.position - battleArenaGameobject.transform.position, playerGameobject.transform.rotation, _photonView.ViewID
        //    };

        //    RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        //    {
        //        Receivers = ReceiverGroup.Others,
        //        CachingOption = EventCaching.AddToRoomCache
        //    };

        //    SendOptions sendOptions = new SendOptions
        //    {
        //        Reliability = true
        //    };

        //    PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode, data, raiseEventOptions, sendOptions);
        //}
        //else
        //{
        //    Debug.Log("Failed to allocate a viewID");
        //    Destroy(playerGameobject);
        //}
    //}
}
