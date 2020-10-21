using Photon.Pun;
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

    public static List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        spawn = FindObjectOfType<Spawn>();
        colours = FindObjectOfType<ColourPalette>();
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), 
            spawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]].position, Quaternion.identity);
        //spawn.AssignSpawnPosition((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);

        PV1 = player.GetComponent<PhotonView>();
        PV1.RPC("RPC_AddToList", RpcTarget.AllBuffered);

        PV2 = player.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<PhotonView>();
        PV2.RPC("RPC_AssignColour", RpcTarget.AllBuffered);
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