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

        StartCoroutine("WaitAndSetParent");
    }

    public void SpawnChests()
    {
        chest = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chest"), spawnPosition.transform.position, spawnPosition.transform.rotation);
    }

    private IEnumerator WaitAndSetParent()
    {
        yield return new WaitForSeconds(1f);
        PV.RPC("SetChestsParent", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetChestsParent()
    {
        if (chest != null)
        {
            chest.transform.parent = gameObject.transform;
        }
        else
        {
            GameObject.Find("Chest(Clone)").transform.parent = gameObject.transform;
        }
    }
}
