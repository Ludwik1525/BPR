using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupController : MonoBehaviour
{
    [SerializeField]
    private int menuSceneIndex;

    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
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