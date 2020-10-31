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
        //Instantiate(chestPrefab, spawnPosition.transform.position, spawnPosition.transform.rotation, gameObject.transform);
        SpawnChests();
    }

    public void SpawnChests()
    {
        chest = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chest"), spawnPosition.transform.position, spawnPosition.transform.rotation);
        chest.transform.parent = gameObject.transform;
    }
}
