using Photon.Pun;
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
        spawn = FindObjectOfType<Spawn>();
        colours = FindObjectOfType<ColourPalette>();
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
                //spawn.AssignSpawnPosition((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]).position, Quaternion.identity);
                Debug.Log("Index " + (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerNo"]);
        PV = player.GetComponentInChildren<PhotonView>();
        PV.RPC("RPC_AssignColour", RpcTarget.AllBuffered);
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