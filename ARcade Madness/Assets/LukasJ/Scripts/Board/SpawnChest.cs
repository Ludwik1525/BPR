using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnChest : MonoBehaviour
{
    public GameObject spawnPosition;
    private GameObject chest;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnChests();
        }

        PV.RPC("SetChestsParent", RpcTarget.AllBuffered);
    }

    public void SpawnChests()
    {
        chest = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chest"), spawnPosition.transform.position, spawnPosition.transform.rotation);
    }

    [PunRPC]
    public void SetChestsParent()
    {
        chest.transform.parent = gameObject.transform;
    }
}
